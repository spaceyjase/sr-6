using System.Configuration;
using Godot;

namespace Player
{
  public class Player : KinematicBody2D
  {
    [Export] private float runSpeed = 150;
    [Export] private float gravity = 750;
    [Export] private float jumpSpeed = -305;
    [Export] private int maxJumps = 1;

    private Timer coyoteTimer;
    private AnimationPlayer animationPlayer;
    private Camera2D camera;

    private string animation;

    public string Animation
    {
      get => animation;
      set
      {
        if (animation == value) return;

        animation = value;
        animationPlayer.Play(animation);
      }
    }

    private Sprite sprite;

    public Vector2 Velocity { get; set; }
    public float Speed { get; private set; }
    public float Gravity { get; private set; }
    public float JumpSpeed { get; private set; }
    public int JumpCount { get; set; }
    public int MaxJumps { get; private set; }
    public bool InCoyoteTime => !coyoteTimer.IsStopped();

    public override void _Ready()
    {
      base._Ready();

      animationPlayer = GetNode<AnimationPlayer>(nameof(AnimationPlayer));
      sprite = GetNode<Sprite>(nameof(Sprite));
      camera = GetNode<Camera2D>(nameof(Camera2D));
      
      coyoteTimer = GetNode<Timer>("CoyoteTimer");

      Speed = runSpeed;
      Gravity = gravity;
      JumpSpeed = jumpSpeed;
      MaxJumps = maxJumps;
    }

    public void Move(Vector2 velocity)
    {
      if (velocity.x < 0)
      {
        sprite.FlipH = true;
      }

      if (velocity.x > 0)
      {
        sprite.FlipH = false;
      }

      Velocity = MoveAndSlide(velocity, Vector2.Up);
    }

    public void SetCameraLimits(Rect2 mapSize, Vector2 cellSize)
    {
      camera.LimitLeft = (int)((mapSize.Position.x - 1) * cellSize.x);
      camera.LimitRight = (int)((mapSize.End.x + 1) * cellSize.x);
      camera.LimitBottom = (int)((mapSize.End.y + 1) * cellSize.y);
      camera.LimitTop = (int)((mapSize.Position.y - 1) * cellSize.y);
    }

    public void StartCoyoteTimer() => coyoteTimer.Start();

    public void CancelCoyoteTime() => coyoteTimer.Stop();
  }
}