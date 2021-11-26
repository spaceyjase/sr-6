using Godot;
using StateMachine;

namespace Player.States
{
  public class PlayerState : State
  {
    protected Player player;

    public override void _Ready()
    {
      base._Ready();

      player = Owner as Player;
    }

    protected void CheckForCollisions()
    {
      for (var i = 0; i < player.GetSlideCount(); ++i)
      {
        var collision = player.GetSlideCollision(i);
        if (!(collision.Collider is KinematicBody2D other) || !other.IsInGroup("Enemies")) continue;
        StateMachine?.ChangeState("Hurt");
        return;
      }
    }
  }
}