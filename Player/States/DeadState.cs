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
    }
  }
}