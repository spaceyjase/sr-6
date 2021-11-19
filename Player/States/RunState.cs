using Godot;

namespace Player.States
{
  public class RunState : PlayerState
  {
    public override void _Ready()
    {
      base._Ready();
      
      OnEnter += () =>
      {
        player.Animation = "run";
      };
      OnPhysicsProcess += PhysicsProcess;
    }

    private void PhysicsProcess(float delta)
    {
      if (player.IsOnFloor())
      {
        player.StartCoyoteTimer();
      }

      var direction = Input.GetActionStrength("right") - Input.GetActionStrength("left");
      var velocity = player.Velocity;
      velocity.x = player.Speed * direction;
      if (!player.InCoyoteTime)
      {
        velocity.y += player.Gravity * delta;
      }
      player.Move(velocity);
      
      if (!player.IsOnFloor() && !player.InCoyoteTime)
      {
        StateMachine?.ChangeState("Jump");
      }

      if (Input.IsActionJustPressed("jump"))
      {
        StateMachine?.ChangeState("Jump");
      }
      else if (player.IsOnFloor() && Mathf.IsEqualApprox(0f, direction))
      {
        StateMachine?.ChangeState("Idle");
      }
    }
  }
}