using Godot;

public class WorldComplete : Control
{
  [Export] private string mainMenuScenePath;

  private bool active;

  public override void _Process(float delta)
  {
    base._Process(delta);

    if (!active) return;

    if (Input.IsActionJustPressed("jump"))
    {
      GetTree().ChangeScene(mainMenuScenePath);
    }
  }

  public void OnAnimationPlayer_Completed()
  {
    active = true;
  }

  public void UpdateGameTime(string time)
  {
    GetNode<Label>("Details/VBoxContainer/GameTimeLabel").Text = time;
  }
}