using System.Diagnostics;
using Godot;
using StateMachine;

namespace Player.States
{
  public class IdleState : PlayerState
  {
    public override void _Ready()
    {
      base._Ready();

      OnEnter += () => { player.Animation = "idle"; };
      OnPhysicsProcess += PhysicsProcess;
    }

    private void PhysicsProcess(float delta)
    {
      var velocity = player.Velocity;
      velocity.x = 0;
      velocity.y += player.Gravity * delta;

      player.Move(velocity);
      for (var i = 0; i < player.GetSlideCount(); ++i)
      {
        var collision = player.GetSlideCollision(i);
        if (!(collision.Collider is KinematicBody2D other) || !other.IsInGroup("Enemies")) continue;
        
        StateMachine?.ChangeState("Hurt");
        return;
      }

      if (!player.IsOnFloor())
      {
        StateMachine?.ChangeState("Jump"); // TODO: air/falling
      }

      var right = Input.IsActionPressed("right");
      var left = Input.IsActionPressed("left");
      var jump = Input.IsActionJustPressed("jump");

      if (left || right)
      {
        StateMachine?.ChangeState("Run");
      }
      else if (jump && player.IsOnFloor())
      {
        StateMachine?.ChangeState("Jump");
      }
    }
  }
}