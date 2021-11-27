using Godot;
using Godot.Collections;

public class VerticalPlatform : KinematicBody2D
{
  [Export] private float speed = 2f;

  private Tween moverTween;
  private Array<Vector2> positions;

  public override void _Ready()
  {
    base._Ready();

    moverTween = GetNode<Tween>("MoverTween");
    moverTween.Connect("tween_completed", this, nameof(OnTweenComplete));
    
    var startPosition = GetNode<Position2D>("StartPosition");
    var endPosition = GetNode<Position2D>("EndPosition");
    
    positions = new Array<Vector2> { startPosition.GlobalPosition, endPosition.GlobalPosition };

    StartTween();
  }

  private void StartTween()
  {
    moverTween.InterpolateProperty(this, "global_position", positions[0], positions[1], speed,
      Tween.TransitionType.Cubic, Tween.EaseType.InOut);
    moverTween.Start();
  }

  private void OnTweenComplete(object o, NodePath key)
  {
    (positions[0], positions[1]) = (positions[1], positions[0]);

    StartTween();
  }
}