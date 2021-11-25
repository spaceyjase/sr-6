using Godot;
using StateMachine;

namespace Player.States
{
  public class HurtState : PlayerState
  {
    public override void _Ready()
    {
      base._Ready();

      OnEnter += OnEnterAction;
    }

    private async void OnEnterAction()
    {
      player.Animation = "hurt";
      player.TakeDamage();
      await ToSignal(GetTree().CreateTimer(player.HurtTimeout), "timeout");
      StateMachine?.ChangeState(player.IsDead ? "Dead" : "Idle");
    }
  }
}