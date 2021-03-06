using Godot;

namespace Props
{
  public class Beam : KinematicBody2D
  {
    [Export] private float growthTime = 0.1f;

    private Line2D line;
    private Tween tween;
    private Timer timer;
    private CollisionShape2D collision;

    private Vector2 endPosition;
    private float initialWidth;

    private bool isCasting;

    private AudioStreamPlayer2D audio;

    private bool IsCasting
    {
      get => isCasting;
      set
      {
        isCasting = value;
        line.Points[1] = isCasting ? endPosition : Vector2.Zero;
        if (isCasting)
        {
          Appear();
        }
        else
        {
          Disappear();
        }
      }
    }

    private void Disappear()
    {
      if (tween.IsActive())
      {
        tween.StopAll();
      }

      tween.InterpolateProperty(line, "width", line.Width, 0, growthTime);
      tween.Start();
      collision.CallDeferred("set", "disabled", true);
      audio.Stop();
    }

    private void Appear()
    {
      if (tween.IsActive())
      {
        tween.StopAll();
      }

      tween.InterpolateProperty(line, "width", 0, initialWidth, growthTime);
      tween.Start();
      collision.CallDeferred("set", "disabled", false);
      audio.Seek(0);
      audio.Play();
    }

    public override void _Ready()
    {
      line = GetNode<Line2D>("Line2D");
      tween = GetNode<Tween>("Tween");
      timer = GetNode<Timer>("Timer");
      collision = GetNode<CollisionShape2D>("CollisionShape2D");
      audio = GetNode<AudioStreamPlayer2D>("Audio");

      endPosition = line.Points[1];
      initialWidth = line.Width;

      var start = GetNode<Node2D>("Start");
      var end = GetNode<Node2D>("End");
      var pos = collision.GlobalPosition;
      var vertical = !Mathf.IsEqualApprox(start.Position.y, end.Position.y);
      if (vertical)
      {
        pos.x = World.World.HalfCellSize;
        pos.y = end.Position.y / 2f + World.World.HalfCellSize;
      }
      else
      {
        pos.y = World.World.HalfCellSize;
        pos.x = end.Position.x / 2f + World.World.HalfCellSize;
      }
      collision.Position = pos;
      var shape = new RectangleShape2D();
      shape.Extents = new Vector2(vertical ? 2f : pos.x, vertical ? pos.y : 2f);
      collision.Shape = shape;

      audio.Position = pos;
        
      // Not initially firing
      IsCasting = false;
    }

    private void OnTimer_Timeout()
    {
      IsCasting = !IsCasting;
    }

    private void OnStartTimer_Timeout()
    {
      timer.Start();
    }
  }
}