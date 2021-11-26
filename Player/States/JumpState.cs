using Godot;

namespace Player.States
{
  public class JumpState : PlayerState
  {
    private Timer jumpTimer;

    public override void _Ready()
    {
      base._Ready();
      
      jumpTimer = GetNode<Timer>("JumpTimer");

      OnPhysicsProcess += PhysicsProcess;
      OnExit += () => { player.JumpCount = 0; };
    }

    private void PhysicsProcess(float delta)
    {
      var direction = Input.GetActionStrength("right") - Input.GetActionStrength("left");
      var velocity = player.Velocity;
      if (!player.WallJumping)
      {
        velocity.x = Mathf.IsEqualApprox(0f, direction)
          ? Mathf.Lerp(velocity.x, 0f, player.Friction)
          : Mathf.Lerp(velocity.x, player.Speed * direction, player.Acceleration);
      }

      var gravityDelta = player.Gravity * delta;
      
      if (velocity.y > 0f && (player.IsTouchingLeftWall || player.IsTouchingRightWall))
      { // player is falling and against a wall
        gravityDelta *= player.WallSlideGravityMultiplier;
        // TODO: particles
      }
      velocity.y += gravityDelta;
      
      var jump = Input.IsActionJustPressed("jump");
      if (jump)
      {
        // jump buffering
        jumpTimer.Start();
      }

      if (player.IsOnFloor() || player.InCoyoteTime)
      {
        // jump!
        player.CancelCoyoteTime();
        velocity.y = player.JumpSpeed;
        player.Animation = "jump_up";
        player.JumpCount++;
      }
      else if (player.JumpCount < player.MaxJumps && jump)
      {
        // double (triple!) jumps
        velocity.y = player.JumpSpeed;
        player.Animation = "jump_up";
        if (player.IsTouchingLeftWall || player.IsTouchingRightWall)
        { // wall jump!
          velocity.x = player.IsTouchingLeftWall ? player.WallJumpSpeed : -player.WallJumpSpeed;
          player.WallJumping = true;
        }
        else
        {
          player.JumpCount++;
        }
      }

      if (Input.IsActionJustReleased("jump") && velocity.y < -player.JumpSpeed * 0.5f)
      {
        // jump cancel
        velocity.y = gravityDelta;
      }

      player.Move(velocity);
      CheckForCollisions();

      if (velocity.y > 0)
      {
        player.Animation = "jump_down";
      }

      if (player.IsOnFloor() && jumpTimer.IsStopped())
      {
        StateMachine?.ChangeState(Mathf.IsEqualApprox(0f, direction) ? "Idle" : "Run");
        // TODO: particles
      }
    }
  }
}