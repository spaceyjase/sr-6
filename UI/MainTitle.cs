using Godot;
using System;

public class MainTitle : Control
{
  [Export] private string worldScenePath;

  public override void _Ready()
  {
    base._Ready();
    GetNode<Button>("PlayButton").GrabFocus();
  }

  private void OnPlayButton_Pressed()
  {
    GetTree().ChangeScene(worldScenePath);
  }
}