using Extensions;
using Godot;
using Pickups.Battery;

namespace Levels
{
  // Base level node for all others to inherit
  public class Level : Node2D
  {
    private TileMap world;

    public Level() { }

    public override void _Ready()
    {
      world = GetNode<TileMap>("World");
      SetProcess(false);
      SetPhysicsProcess(false);

      foreach (var battery in this.GetChildren<Battery>())
      {
        battery.Connect(nameof(Battery.Pickup), this, nameof(OnBatteryPickup));
      }
    }
    
    public Area2D Area => GetNode<Area2D>("Area2D");

    public Rect2 GetUsedRect() => world.GetUsedRect();
    public Vector2 GetCellSize() => world.CellSize;

    private void OnBatteryPickup()
    {
      GD.Print("Battery collected!");
    }
  }
}