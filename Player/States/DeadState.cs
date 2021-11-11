using Godot;
using StateMachine;

namespace Player.States
{
  public class DeadState : State
  {
    public override void _Ready()
    {
      base._Ready();

      OnProcess += () => { GD.Print(Name); };
    }
  }
}