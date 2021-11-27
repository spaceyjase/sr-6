using Godot;
using System;

public class MainTitle : Control
{
  [Export] private string worldScenePath;

  private void OnPlayButton_Pressed()
  {
    GetTree().ChangeScene(worldScenePath);
  }
}