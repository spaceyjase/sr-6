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
      velocity.x = Mathf.IsEqualApprox(0f, direction)
        ? Mathf.Lerp(velocity.x, 0f, player.Friction)
        : Mathf.Lerp(velocity.x, player.Speed * direction, player.Acceleration);
      if (!player.InCoyoteTime)
      {
        velocity.y += player.Gravity * delta;
      }
      player.Move(velocity);
      for (var i = 0; i < player.GetSlideCount(); ++i)
      {
        var collision = player.GetSlideCollision(i);
        if (!(collision.Collider is KinematicBody2D other) || !other.IsInGroup("Enemies")) continue;
        
        StateMachine?.ChangeState("Hurt");
        return;
      }
      
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