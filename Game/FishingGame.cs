using Godot;
using System;

public partial class FishingGame : Control
{
	[Export]public float MarkerSpeed = 250f;
	
	private Button _menu;
	private Panel _bar;
	private ColorRect _zone;
	private ColorRect _marker;
	
	private bool _movingRight =true;
	private bool _inputLocked = false;
	
	private enum Phase{Waiting, Aiming, Transition, FinalSmash, Fail, Win}
	private Phase _phase = Phase.Aiming;
	
	private readonly RandomNumberGenerator _rng = new();
	
	private int _successCount = 0;
	private const int MaxSuccess =5;
	private int _mashCount = 0;
	private const int MashGoal = 15;
	
	private Timer _waitTimer;
	private Random _rng2 = new Random();
	
	private ProgressBar _smashBar;
	private Timer _smashTimer;
	[Export] public float SmashTimeLimit = 5f;
	
	private FishData _currentFish;
	
	public override void _Ready()
	{
		_bar = GetNode<Panel>("Panel");
		_zone = GetNode<ColorRect>("Panel/ZoneRouge");
		_marker = GetNode<ColorRect>("Panel/Marqueur");
		_menu = GetNode<Button>("VBoxContainer/Menu");
		_smashBar = GetNode<ProgressBar>("SmashBar");
		_smashTimer = GetNode<Timer>("SmashTimer");
		_waitTimer = GetNode<Timer>("WaitTime");
		_smashTimer.Timeout += OnSmashTimeOut;
		_waitTimer.Timeout += OnWaitFinished;
		
		StartWaitingPhase();
		
		_menu.Pressed += OnMenuPressed;
		
		_rng.Randomize();
		PlaceRandomZone();
		
	}
	
	public override void _Process(double delta)
	{
		if(_phase == Phase.Aiming)
		{
			MoveMarker((float)delta);
			
			if(!_inputLocked && Input.IsActionJustPressed("ui_accept"))
			{
			_inputLocked = true;
			HandleHit();
			}
		}
		else if(_phase == Phase.FinalSmash)
		{
			if(Input.IsActionJustPressed("ui_accept"))
			{
				_mashCount++;
				_smashBar.Value = _mashCount;
				GD.Print($"Smashing {_mashCount}/{_smashBar.MaxValue}");
				if(_mashCount >= _smashBar.MaxValue)
				{
					_smashTimer.Stop();
					WinGame();
				}
			}
		}
	
	}
	
	private void StartWaitingPhase()
	{
		_phase = Phase.Waiting;
		GD.Print("Waiting for a big fish");
		
		_currentFish = FishDataBase.GetRandomFishByRarity();
		GD.Print($"A { _currentFish.Rarity } fish is near by...");
		
		_marker.Visible = false;
		_zone.Visible = false;
		_bar.Visible = false;
		
		var (min,max) = FishDifficulty.GetWaitingTimeRange(_currentFish.Rarity);
		float waitTime = (float)(_rng.Randf() * (max - min) + min);
		
		_waitTimer.WaitTime = waitTime;
		_waitTimer.Start();
		
		GD.Print($"A {_currentFish.Rarity} fish -> Waiting {waitTime:F1}s before a bite..");
	}
	
	private void OnWaitFinished()
	{
		GD.Print("A Bite !!!");
		StartFishingPhase();
	}
	
	private void StartFishingPhase()
	{
		_phase = Phase.Aiming;
		_successCount = 0;
		
		_marker.Visible = true;
		_zone.Visible = true;
		_bar.Visible =true;
		
		PlaceRandomZone();
	}
	
	private void MoveMarker(float dt)
	{
		float move = MarkerSpeed * dt;
		var pos = _marker.Position;
		pos.X += _movingRight ? move : -move;
		
		if(pos.X <= 0)
		{
			pos.X =0;
			_movingRight = true;
		}
		else if( pos.X + _marker.Size.X >= _bar.Size.X)
		{
			pos.X = _bar.Size.X - _marker.Size.X;
			_movingRight = false;
		}
		 _marker.Position = pos;
	}
	
	private async void HandleHit()
	{
		Vector2 center = _marker.Position + _marker.Size /2f;
		Rect2 zoneRect = new Rect2(_zone.Position,_zone.Size);
		
		if(zoneRect.HasPoint(center))
		{
			_successCount ++;
			GD.Print($"sucess | centerX={center.X:F1} zone=[{_zone.Position.X:F1}..{(_zone.Position.X +_zone.Size.X):F1}]");
			
			if(_successCount >= MaxSuccess)
			{
				StartFinalPhase();
				return;
			}
			
			_phase = Phase.Transition;
			await ToSignal(GetTree(),SceneTree.SignalName.ProcessFrame);
			PlaceRandomZone();
			_phase = Phase.Aiming;
			_inputLocked = false;
		}
		else
		{
			GD.Print($"Failure -> Menu | centerX={center.X:F1} zone=[{_zone.Position.X:F1}..{(_zone.Position.Y +_zone.Size.X):F1}]");
			_phase = Phase.Fail;
			await ToSignal(GetTree(),SceneTree.SignalName.ProcessFrame);
			GetTree().ChangeSceneToFile("res://main_menu.tscn");
		}
	}
	
	private void PlaceRandomZone()
	{
		float zoneWidth = _bar.Size.X * 0.3f;
		
		switch (_currentFish.Rarity)
		{
			case FishRarity.Common : zoneWidth *= 1.1f; break;
			case FishRarity.Uncommon : break;
			case FishRarity.Rare : zoneWidth *= 0.8f; break;
			case FishRarity.Epic : zoneWidth *= 0.6f; break;
			case FishRarity.Legendary : zoneWidth *= 0.4f; break;
		}
		
		float x = (float)_rng.Randf() * (_bar.Size.X - zoneWidth);
		_zone.Size = new Vector2(zoneWidth, _zone.Size.Y);
		_zone.Position = new Vector2(x, _zone.Position.Y);
	}
	
	private void OnMenuPressed()
	{
		GD.Print("Menu");
			_phase = Phase.Fail;
			GetTree().ChangeSceneToFile("res://main_menu.tscn");
	}
	
	private void StartFinalPhase()
	{
		GD.Print("Finale Phase !");
		_phase = Phase.FinalSmash;
		_mashCount = 0;
		
		_marker.Position = new Vector2((_bar.Size.X - _marker.Size.X)/ 2f, _marker.Position.Y);
		_zone.Visible = false;
		
		 int goal = FishDifficulty.GetMashGoal(_currentFish.Rarity);
		 float time = FishDifficulty.GetMashTime(_currentFish.Rarity);
		
		_smashBar.Visible = true;
		_smashBar.Value = 0;
		_smashBar.MaxValue = goal;
		
		_smashTimer.WaitTime = time;
		_smashTimer.Start();
		
		GD.Print($"A {_currentFish.Rarity} fish -> Goal: {goal} smashes in {time}s");
	}
	
	private void WinGame()
	{
		_phase = Phase.Win;
		GD.Print($"You caught a {_currentFish.Name}, a {_currentFish.Rarity} fish, of {_currentFish.Weight}KG, Temperament : {_currentFish.Temperament}.");
		PlayerData.Instance.AddFish(_currentFish);
		GetTree().ChangeSceneToFile("res://Winning/WinningScene.tscn");
	}
	
	private void OnSmashTimeOut()
	{
		if(_phase == Phase.FinalSmash)
		{
			GD.Print($"Time is over... It's been {_smashTimer.WaitTime} secondes");
			_phase = Phase.Fail;
			GetTree().ChangeSceneToFile("res://main_menu.tscn");
		}
	}
}
