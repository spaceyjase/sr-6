using Godot;

namespace Pickups.Battery
{
  public class Battery : Node2D
  {
    [Signal] public delegate void Pickup();

    private void OnArea2D_Body_Entered(Node2D body)
    {
      // Only the player should get here...
      EmitSignal(nameof(Pickup));
      QueueFree();
    }
  }
}