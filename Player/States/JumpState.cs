using Godot;

namespace Player.States
{
  public class JumpState : PlayerState
  {
    private Timer coyoteTimer;
    public override void _Ready()
    {
      base._Ready();
      
      coyoteTimer = GetNode<Timer>("CoyoteTimer");

      OnEnter += () =>
      {
        coyoteTimer.Start();
      };
      OnPhysicsProcess += PhysicsProcess;
      OnExit += () => { player.JumpCount = 0; };
    }

    private void PhysicsProcess(float delta)
    {
      var direction = Input.GetActionStrength("right") - Input.GetActionStrength("left");
      var velocity = player.Velocity;
      velocity.x = player.Speed * direction;
      if (coyoteTimer.IsStopped())
      {
        velocity.y += player.Gravity * delta;
      }

      if (player.IsOnFloor())
      {
        velocity.y = player.JumpSpeed;
        player.Animation = "jump_up";
        player.JumpCount++;
      }
      else if (player.JumpCount < player.MaxJumps && Input.IsActionJustPressed("jump"))
      {
        velocity.y = player.JumpSpeed;
        player.Animation = "jump_up";
        player.JumpCount++;
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