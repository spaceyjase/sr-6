using Godot;
using StateMachine;

namespace Player.States
{
  public class IdleState : State
  {
    public override void _Ready()
    {
      base._Ready();

      OnProcess += () => { GD.Print(Name); };
    }
  }
}