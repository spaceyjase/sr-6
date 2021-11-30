using System.Collections.Generic;
using System.Linq;
using Godot;
using Levels;
using Extensions;
using Godot.Collections;

namespace World
{
  public class World : Node2D
  {
    [Export] private PackedScene worldCompleteScene;
    
    public const int HalfCellSize = 4;

    private readonly List<Level> levels = new List<Level>();
    private UI.HUD hud;
    private int totalBatteries;

    private AudioStreamPlayer pickupAudio;
    private Particles2D pickupParticles;
    private AnimationPlayer faderAnimationPlayer;

    private bool worldComplete = false;
    private int batteries;

    public override void _Ready()
    {
      // Get total number of batteries in the world
      var batteryNode = GetTree().GetNodesInGroup("Battery");
      totalBatteries = batteryNode.Count;
      GD.Print($"Total batteries: {totalBatteries}");

      hud = GetNode<UI.HUD>("CanvasLayer/HUD");
      hud.UpdateScore(totalBatteries);
      
      var levelParent = GetNode<Node2D>("LevelParent");
      foreach (var level in levelParent.GetChildren<Level>())
      {
        levels.Add(level);
        level.Area.Connect("body_entered", this, nameof(OnLevelBodyEntered), new Array { level });
        level.Connect(nameof(Level.BatteryCollected), this, nameof(OnLevel_BatteryCollected));
      }

      ResetPlayer();

      pickupAudio = GetNode<AudioStreamPlayer>("PickupAudio");
      pickupParticles = GetNode<Particles2D>("PickupParticles");

      faderAnimationPlayer = GetNode<AnimationPlayer>("CanvasLayer/FaderAnimationPlayer");
    }

    private void ResetPlayer()
    {
      var player = GetNode<Player.Player>("Player");
      player.Visible = true;
      var start = levels.First((level => level.Name == "Start"));
      var newBounds = start.GetUsedRect();
      newBounds.Position += start.Position;
      player.SetCameraLimits(newBounds, start.GetCellSize());
    }

    private void OnLevel_BatteryCollected(Vector2 position)
    {
      pickupParticles.GlobalPosition = position;
      pickupParticles.Emitting = true;
      pickupAudio.Play();
      pickupAudio.PitchScale = (float)GD.RandRange(0.9f, 1.1f);
      hud.UpdateScore(totalBatteries);

      if (++batteries < totalBatteries) return;

      worldComplete = true;
    }

    private void OnLevelBodyEntered(Node2D area, Level level)
    {
      // Should be the player...
      var newBounds = level.GetUsedRect();
      newBounds.Position += level.Position;
      GetNode<Player.Player>("Player")?.SetCameraLimits(newBounds, level.GetCellSize());
    }

    private void OnPlayer_Dead()
    {
      hud.Stop();
      faderAnimationPlayer.Play("fade_in_death");

      if (OS.IsDebugBuild())
      {
        GD.Print($"Game Over at {hud.GameTimeAsString}");
      }
    }

    private void OnFaderAnimationPlayer_Animation_Finished(string animation)
    {
      if (animation == "fade_in_death")
      {
        GetTree().ChangeScene("res://UI/MainTitle.tscn");
      }
    }

    private void OnSpaceship_Body_Entered(Node area)
    {
      if (!worldComplete) return;
      
      // World complete!
      GetNode<AnimationPlayer>("Spaceship/AnimationPlayer").Play("take_off");
      GetNode<Player.Player>("Player").IsActive = false;
      faderAnimationPlayer.Play("fade_in_world_complete");
      hud.Stop();
      
      // Instance in a world complete scene
      var instance = worldCompleteScene.Instance<WorldComplete>();
      GetNode<CanvasLayer>("CanvasLayer").AddChild(instance);
      instance.UpdateGameTime(hud.GameTimeAsString);

      worldComplete = false;
    }
  }
}