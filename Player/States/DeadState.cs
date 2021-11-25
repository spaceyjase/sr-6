using Godot;
using StateMachine;

namespace Player.States
{
  public class DeadState : PlayerState
  {
    public override void _Ready()
    {
      base._Ready();
      OnEnter += () => { player.Animation = "dead"; };
      OnPhysicsProcess += PhysicsProcess;
    }

    private void PhysicsProcess(float delta)
    {
      var velocity = player.Velocity;
      var gravityDelta = player.Gravity * delta;
      velocity.y += gravityDelta;
      velocity.x = Mathf.Lerp(velocity.x, 0f, player.Friction * delta);
      
      player.Move(velocity, true);
    }
  }
}