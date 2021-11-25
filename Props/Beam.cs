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
    }

    public override void _Ready()
    {
      line = GetNode<Line2D>("Line2D");
      tween = GetNode<Tween>("Tween");
      timer = GetNode<Timer>("Timer");
      collision = GetNode<CollisionShape2D>("CollisionShape2D");

      endPosition = line.Points[1];
      initialWidth = line.Width;

      // TODO: automatically set the start and end positions for the line (currently editing children manually)

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