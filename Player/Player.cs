using System.Runtime.Remoting.Lifetime;
using Godot;

namespace Player
{
  public class Player : KinematicBody2D
  {
    [Signal]
    private delegate void LifeChanged(int life);

    [Signal]
    private delegate void Dead();

    [Export] private int initialLife = 3;
    [Export] private float runSpeed = 85;
    [Export] private float gravity = 700;
    [Export] private float jumpSpeed = -150;
    [Export] private int maxJumps = 2;
    [Export] private float friction = 0.75f;
    [Export] private float acceleration = 0.1f;
    [Export] private float wallSlideGravityMultiplier = 0.33f;
    [Export] private float wallJumpSpeed = 50f;
    [Export] private float hurtTimeout = 0.5f;
    [Export] private float hurtShake = 0.1f;
    [Export] private int outOfWorld = 3000;

    private Timer coyoteTimer;
    private Timer wallJumpTimer;
    private Timer invulnerableTimer;
    private AnimationPlayer animationPlayer;
    private Camera2D camera;
    private CameraShake cameraShake;
    private RayCast2D raycastLeft;
    private RayCast2D raycastRight;

    private AudioStreamPlayer jumpAudio;
    private AudioStreamPlayer hurtAudio;
    private AudioStreamPlayer deathAudio;

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
    private int life;

    private int Life
    {
      get => life;
      set
      {
        life = value;
        EmitSignal(nameof(LifeChanged), life);
      }
    }

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
    private bool active;

    public bool WallJumping
    {
      get => !wallJumpTimer.IsStopped();
      set
      {
        if (value) wallJumpTimer.Start();
      }
    }

    private bool IsInvulnerable
    {
      get => !invulnerableTimer.IsStopped();
      set
      {
        if (!value) return;

        invulnerableTimer.Start();
        var modulate = sprite.SelfModulate;
        modulate.a = 0.5f;
        sprite.SelfModulate = modulate;
      }
    }

    public float HurtTimeout => hurtTimeout;

    public override void _Ready()
    {
      base._Ready();

      animationPlayer = GetNode<AnimationPlayer>(nameof(AnimationPlayer));
      sprite = GetNode<Sprite>(nameof(Sprite));
      camera = GetNode<Camera2D>(nameof(Camera2D));
      cameraShake = GetNode<CameraShake>(nameof(Camera2D));
      coyoteTimer = GetNode<Timer>("CoyoteTimer");
      wallJumpTimer = GetNode<Timer>("WallJumpTimer");
      invulnerableTimer = GetNode<Timer>("InvulnerableTimer");
      raycastLeft = GetNode<RayCast2D>("RayCast2DLeft");
      raycastRight = GetNode<RayCast2D>("RayCast2DRight");

      jumpAudio = GetNode<AudioStreamPlayer>("JumpAudio");
      hurtAudio = GetNode<AudioStreamPlayer>("HurtAudio");
      deathAudio = GetNode<AudioStreamPlayer>("DeathAudio");

      Life = initialLife;

      Speed = runSpeed;
      Gravity = gravity;
      JumpSpeed = jumpSpeed;
      MaxJumps = maxJumps;

      IsActive = true;
    }

    public void Move(Vector2 velocity, bool hurtOrDead = false)
    {
      if (velocity.x < 0)
      {
        if (!hurtOrDead)
        {
          sprite.FlipH = true;
          raycastLeft.Enabled = true;
          raycastRight.Enabled = false;
        }
      }

      if (velocity.x > 0)
      {
        if (!hurtOrDead)
        {
          sprite.FlipH = false;
          raycastLeft.Enabled = false;
          raycastRight.Enabled = true;
        }
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

    private void OnInvulnerableTimer_Timeout()
    {
      var modulate = sprite.SelfModulate;
      modulate.a = 1.0f;
      sprite.SelfModulate = modulate;
    }

    public void TakeDamage()
    {
      if (IsInvulnerable) return;

      Life -= 1;
      EmitSignal(nameof(LifeChanged), Life);
      if (!IsDead)
      {
        hurtAudio.Play();
        hurtAudio.PitchScale = (float)GD.RandRange(0.9f, 1.1f);
        cameraShake.AddTrauma(hurtShake);
        IsInvulnerable = true;
        return;
      }

      deathAudio.Play();
      EmitSignal(nameof(Dead));
      SetProcess(false);
      SetPhysicsProcess(false);
    }

    public bool OutOfBounds()
    {
      var result = Position.y > outOfWorld;
      if (result)
      {
        deathAudio.Play();
        EmitSignal(nameof(Dead));
        SetProcess(false);
        SetPhysicsProcess(false);
      }

      return result;
    }

    public bool IsDead => Life <= 0;
    public bool IsFacingLeft => sprite.FlipH;

    public void Jump()
    {
      jumpAudio.Play();
      jumpAudio.PitchScale = (float)GD.RandRange(0.9f, 1.1f);
    }

    public bool IsActive
    {
      get => active;
      set
      {
        active = value;
        Visible = active;
      }
    }
  }
}