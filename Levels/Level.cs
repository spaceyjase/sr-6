using Godot;

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
    }
    
    public Area2D Area => GetNode<Area2D>("Area2D");

    public Rect2 GetUsedRect() => world.GetUsedRect();
    public Vector2 GetCellSize() => world.CellSize;
  }
}