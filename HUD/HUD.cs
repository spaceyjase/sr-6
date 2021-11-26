using Godot;
using Godot.Collections;

namespace HUD
{
  public class HUD : MarginContainer
  {
    private Array<TextureRect> lifeCounter;
    private Label batteryLabel;
    private int batteryCount;
  
    public override void _Ready()
    {
      lifeCounter = new Array<TextureRect>
      {
        GetNode<TextureRect>("HBoxContainer/LifeCounter/L1"),
        GetNode<TextureRect>("HBoxContainer/LifeCounter/L2"),
        GetNode<TextureRect>("HBoxContainer/LifeCounter/L3")
      };
      batteryLabel = GetNode<Label>("HBoxContainer/BatteryDisplay/BatteryLabel");
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
    }

    public void UpdateScore(int batteryTotal)
    {
      batteryLabel.Text = $"{++batteryCount} / {batteryTotal}";
    }
  }
}