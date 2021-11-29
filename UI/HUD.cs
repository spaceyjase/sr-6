using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Godot;
using Godot.Collections;

namespace UI
{
  public class HUD : Control
  {
    private Array<TextureRect> lifeCounter;
    private Label batteryLabel;
    private Label gameTimeLabel;
    private int batteryCount = -1; // HACK for initial score update.
    private AnimationPlayer animationPlayer;

    private double timer;

    private bool stopped;

    public override void _Ready()
    {
      lifeCounter = new Array<TextureRect>
      {
        GetNode<TextureRect>("PlayerDetails/HBoxContainer/LifeCounter/L1"),
        GetNode<TextureRect>("PlayerDetails/HBoxContainer/LifeCounter/L2"),
        GetNode<TextureRect>("PlayerDetails/HBoxContainer/LifeCounter/L3")
      };
      gameTimeLabel = GetNode<Label>("GameTime/GameTimeLabel");
      batteryLabel = GetNode<Label>("PlayerDetails/HBoxContainer/HBoxContainer/BatteryLabel");
      animationPlayer = GetNode<AnimationPlayer>("PlayerDetails/AnimationPlayer");
    }

    public override void _Process(float delta)
    {
      base._Process(delta);

      if (stopped) return;
      
      timer += delta;

      gameTimeLabel.Text = FormatTimer();
    }

    private string FormatTimer()
    {
      // format output in minutes and hours from seconds
      var minutes = Math.Floor(timer / 60);
      var hours = (int)Math.Floor(minutes / 60);
      minutes %= 60;
      
      return $"{hours:00}:{minutes:00}:{timer % 60:00.00}";
    }

    public void Stop()
    {
      stopped = true;
      SetProcess(false);
    }

    public string GameTimeAsString => FormatTimer();

    private async void OnPlayer_Life_Changed(int value)
    {
      if (lifeCounter == null)
      {
        await ToSignal(this, "ready");
      }

      for (var index = 0; index < lifeCounter.Count; ++index)
      {
        lifeCounter[index].Visible = value > index;
      }

      if (value != lifeCounter.Count)
      {
        animationPlayer.Play("update_life");
      }
    }

    public void UpdateScore(int batteryTotal)
    {
      batteryLabel.Text = $"{++batteryCount} / {batteryTotal}";
      if (batteryCount == 0) return;
      animationPlayer.Play("update_score");
    }
  }
}