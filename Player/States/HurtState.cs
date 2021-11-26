using Godot;

namespace Player.States
{
  public class HurtState : PlayerState
  {
    private Vector2 velocity;

    public override void _Ready()
    {
      base._Ready();

      OnEnter += OnEnterAction;
      OnPhysicsProcess += PhysicsProcess;
    }

    private void PhysicsProcess(float delta)
    {
      var gravityDelta = player.Gravity * delta;
      velocity.y += gravityDelta;

      player.Move(velocity, true);
    }

    private async void OnEnterAction()
    {
      velocity = player.Velocity;
      velocity.x = player.IsFacingLeft ? player.WallJumpSpeed : -player.WallJumpSpeed;
      velocity.y = player.JumpSpeed;

      player.Animation = "hurt";
      player.TakeDamage();
      await ToSignal(GetTree().CreateTimer(player.HurtTimeout), "timeout");
      StateMachine?.ChangeState(player.IsDead ? "Dead" : "Idle");
    }
  }
}