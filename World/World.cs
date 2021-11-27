using System.Collections.Generic;
using Godot;
using Levels;
using Extensions;
using Godot.Collections;

namespace World
{
  public class World : Node2D
  {
    public const int HalfCellSize = 4;
    
    private readonly List<Level> levels = new List<Level>();
    private HUD.HUD hud;
    private int totalBatteries;

    private AudioStreamPlayer pickupAudio;
    private Particles2D pickupParticles;
    
    public override void _Ready()
    {
      // Get total number of batteries in the world
      var batteries = GetTree().GetNodesInGroup("Battery");
      totalBatteries = batteries.Count;
      GD.Print($"Total batteries: {totalBatteries}");
      
      hud = GetNode<HUD.HUD>("CanvasLayer/HUD");
      hud.UpdateScore(totalBatteries);
      
      var levelParent = GetNode<Node2D>("LevelParent");
      foreach (var level in levelParent.GetChildren<Level>())
      {
        levels.Add(level);
        level.Area.Connect("body_entered", this, nameof(OnLevelBodyEntered), new Array{ level });
        level.Connect(nameof(Level.BatteryCollected), this, nameof(OnLevel_BatteryCollected));
      }
      
      // TODO: spawn player? ...or at least enable controls after intro animation.
      GetNode<Player.Player>("Player").Visible = true;
      
      pickupAudio = GetNode<AudioStreamPlayer>("PickupAudio");
      pickupParticles = GetNode<Particles2D>("PickupParticles");
    }

    private void OnLevel_BatteryCollected(Vector2 position)
    {
      pickupParticles.GlobalPosition = position;
      pickupParticles.Emitting = true;
      pickupAudio.Play();
      pickupAudio.PitchScale = (float)GD.RandRange(0.9f, 1.1f);
      hud.UpdateScore(totalBatteries);
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
      GD.Print("Player died");
      
      // TODO: game over
    }
  }
}