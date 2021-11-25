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
    
    private List<Level> levels = new List<Level>();
    
    public override void _Ready()
    {
      var levelParent = GetNode<Node2D>("LevelParent");
      foreach (var level in levelParent.GetChildren<Level>())
      {
        levels.Add(level);
        level.Area.Connect("body_entered", this, nameof(OnLevelBodyEntered), new Array{ level });
      }
      GetNode<Player.Player>("Player").Visible = true;
      
      // TODO: spawn player
    }

    private void OnLevelBodyEntered(Node2D area, Level level)
    {
      // Should be the player...
      var newBounds = level.GetUsedRect();
      newBounds.Position += level.Position;
      GetNode<Player.Player>("Player")?.SetCameraLimits(newBounds, level.GetCellSize());
    }
  }
}