using Godot;

namespace Player
{
  public class Player : KinematicBody2D
  {
    [Export] private float runSpeed = 85;
    [Export] private float gravity = 700;
    [Export] private float jumpSpeed = -150;
    [Export] private int maxJumps = 2;
    [Export] private float friction = 0.75f;
    [Export] private float acceleration = 0.1f;
    [Export] private float wallSlideGravityMultiplier = 0.33f;
    [Export] private float wallJumpSpeed = 50f;

    private Timer coyoteTimer;
    private Timer wallJumpTimer;
    private AnimationPlayer animationPlayer;
    private Camera2D camera;
    private RayCast2D raycastLeft;
    private RayCast2D raycastRight;

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

    public float Friction => friction;
    public float Acceleration => acceleration;
    public Vector2 Velocity { get; private set; }
    public float Speed { get; private set; }
    public float Gravity { get; private set; }
    public float JumpSpeed { get; private set; }
    public int JumpCount { get; set; }
    public int MaxJumps { get; private set; }
    public bool InCoyoteTime => !coyoteTimer.IsStopped();
    public bool IsTouchingLeftWall => raycastLeft.IsColliding();
    public bool IsTouchingRightWall => raycastRight.IsColliding();
    public float WallSlideGravityMultiplier => wallSlideGravityMultiplier;
    public float WallJumpSpeed => wallJumpSpeed;

    public bool WallJumping
    {
      get => !wallJumpTimer.IsStopped();
      set
      {
        if (value) wallJumpTimer.Start();
      }
    }

    public override void _Ready()
    {
      base._Ready();

      animationPlayer = GetNode<AnimationPlayer>(nameof(AnimationPlayer));
      sprite = GetNode<Sprite>(nameof(Sprite));
      camera = GetNode<Camera2D>(nameof(Camera2D));
      coyoteTimer = GetNode<Timer>("CoyoteTimer");
      wallJumpTimer = GetNode<Timer>("WallJumpTimer");
      raycastLeft = GetNode<RayCast2D>("RayCast2DLeft");
      raycastRight = GetNode<RayCast2D>("RayCast2DRight");

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
      camera.LimitLeft = (int)mapSize.Position.x;
      camera.LimitRight = (int)(mapSize.Position.x + mapSize.Size.x * cellSize.x);
      camera.LimitBottom = (int)(mapSize.Position.y + mapSize.Size.y * cellSize.y);
      camera.LimitTop = (int)mapSize.Position.y;
    }

    public void StartCoyoteTimer() => coyoteTimer.Start();

    public void CancelCoyoteTime() => coyoteTimer.Stop();
  }
}