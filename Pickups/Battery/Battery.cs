using Godot;

namespace Pickups.Battery
{
  public class Battery : Node2D
  {
    [Signal] public delegate void Pickup(Vector2 position);

    private void OnArea2D_Body_Entered(Node2D body)
    {
      // Only the player should get here...
      EmitSignal(nameof(Pickup), GlobalPosition);
      QueueFree();
    }
  }
}