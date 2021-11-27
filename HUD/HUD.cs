using Godot;
using Godot.Collections;

namespace HUD
{
  public class HUD : MarginContainer
  {
    private Array<TextureRect> lifeCounter;
    private Label batteryLabel;
    private int batteryCount = -1;  // HACK for initial score update.
    private AnimationPlayer animationPlayer;
  
    public override void _Ready()
    {
      lifeCounter = new Array<TextureRect>
      {
        GetNode<TextureRect>("HBoxContainer/LifeCounter/L1"),
        GetNode<TextureRect>("HBoxContainer/LifeCounter/L2"),
        GetNode<TextureRect>("HBoxContainer/LifeCounter/L3")
      };
      batteryLabel = GetNode<Label>("HBoxContainer/HBoxContainer/BatteryLabel");
      animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    private async void OnPlayer_Life_Changed(int value)
    {
      if (lifeCounter == null)
      {
        await ToSignal(this, "ready");
      }

      for (var index = 0; index < lifeCounter.Count; ++index)
      {
        lifeCounter[index].Visible = value > index;
      }

      animationPlayer.Play("update_life");
    }

    public void UpdateScore(int batteryTotal)
    {
      batteryLabel.Text = $"{++batteryCount} / {batteryTotal}";
      if (batteryCount == 0) return;
      animationPlayer.Play("update_score");
    }
  }
}