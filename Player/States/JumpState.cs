using Godot;

namespace Player.States
{
  public class JumpState : PlayerState
  {
    public override void _Ready()
    {
      base._Ready();

      OnEnter += () =>
      {
        if (!player.IsOnFloor())
        {
          player.JumpCount++;
        }
      };
      OnPhysicsProcess += PhysicsProcess;
      OnExit += () => { player.JumpCount = 0; };
    }

    private void PhysicsProcess(float delta)
    {
      var direction = Input.GetActionStrength("right") - Input.GetActionStrength("left");
      var velocity = player.Velocity;
      velocity.x = player.Speed * direction;
      velocity.y += player.Gravity * delta;

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