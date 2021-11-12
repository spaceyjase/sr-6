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
  }
}