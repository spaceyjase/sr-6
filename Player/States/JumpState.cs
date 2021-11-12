using Godot;
using StateMachine;

namespace Player.States
{
  public class JumpState : PlayerState
  {
    public override void _Ready()
    {
      base._Ready();

      OnEnter += () =>
      {
        player.Animation = "jump_up";
        var velocity = player.Velocity;
        velocity.y = player.JumpSpeed;
        player.Velocity = velocity;
      };
      OnPhysicsProcess += PhysicsProcess;
    }

    private void PhysicsProcess(float delta)
    {
      var direction = Input.GetActionStrength("right") - Input.GetActionStrength("left");
      var jump = Input.IsActionPressed("jump");

      var velocity = player.Velocity;
      velocity.y += player.Gravity * delta;

      if (jump) // TODO: double jump
      {
        velocity.x = player.Speed * direction;
        velocity.y = player.JumpSpeed / 1.5f;
      }
      player.Move(velocity);

      if (velocity.y > 0)
      {
        player.Animation = "jump_down";
      }

      if (player.IsOnFloor())
      {
        StateMachine?.ChangeState(Mathf.IsEqualApprox(0f, direction) ? "Idle" : "Run");
        // TODO: particles
      }
    }
  }
}