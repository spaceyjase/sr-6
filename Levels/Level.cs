using Godot;

namespace Levels
{
  // Base level node for all others to inherit
  public class Level : Node2D
  {
    private TileMap world;
    private CollisionShape2D boundary;
    
    public override void _Ready()
    {
      world = GetNode<TileMap>("World");
      boundary = GetNode<CollisionShape2D>("Boundary/CollisionShape2D");
      var shape = new RectangleShape2D();
      var end = world.GetUsedRect().End;
      shape.Extents = new Vector2(end.x * world.CellSize.x / 2, end.y * world.CellSize.x / 2);
      boundary.Shape = shape;
    }

    private void OnBoundary_Body_Entered(Node area)
    {
      (area as Player.Player)?.SetCameraLimits(world.GetUsedRect(), world.CellSize);
    }
    
    private void OnBoundary_Body_Exited(Node area)
    {
      GD.Print("Exited level: " + area.Name);
    }
  }
}
