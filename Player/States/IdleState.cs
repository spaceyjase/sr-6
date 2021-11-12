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

      OnEnter += () =>
      {
        player.Animation = "idle";
      };
      OnPhysicsProcess += PhysicsProcess;
    }

    private void PhysicsProcess(float delta)
    {
      var velocity = player.Velocity;
      velocity.y += player.Gravity * delta;
      
      if (!player.IsOnFloor())
      {
        StateMachine?.ChangeState("Jump"); // TODO: air/falling
      }
      
      player.Move(velocity);

      var right = Input.IsActionPressed("right");
      var left = Input.IsActionPressed("left");
      var jump = Input.IsActionPressed("jump");

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