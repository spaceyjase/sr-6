using Godot;

namespace Props
{
  public class Beam : Node2D
  {
    [Export] private float growthTime = 0.1f;
    [Export] private float startDelay;

    private Line2D line;
    private Tween tween;
    private Timer timer;
    private Area2D area;

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
    }

    private void Appear()
    {
      if (tween.IsActive())
      {
        tween.StopAll();
      }

      tween.InterpolateProperty(line, "width", 0, initialWidth, growthTime);
      tween.Start();
    }

    public override void _Ready()
    {
      line = GetNode<Line2D>("Line2D");
      tween = GetNode<Tween>("Tween");
      timer = GetNode<Timer>("Timer");
      area = GetNode<Area2D>("Area2D");

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
  
    private void OnTween_TweenStep(object o, NodePath key, float elapsed, object value)
    {
      switch (area.Monitoring)
      {
        case false when (float)value > initialWidth / 2f:
          area.Monitoring = true;
          break;
        case true:
          area.Monitoring = false;
          break;
      }
    }
  }
}