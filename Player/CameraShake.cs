using Godot;

namespace Player
{
  public class CameraShake : Camera2D
  {
    [Export] private float decay = 0.8f;
    [Export] private Vector2 maxOffset = new Vector2(100, 75);
    [Export] private float maxRoll = 0.1f;

    private float trauma;
    private int traumaPower = 2;
    private OpenSimplexNoise noise;
    private int noiseY;

    public override void _Ready()
    {
      base._Ready();

      GD.Randomize();
    
      noise = new OpenSimplexNoise();
      noise.Seed = (int)GD.Randi();
      noise.Period = 4;
      noise.Octaves = 2;
    }

    public void AddTrauma(float amount)
    {
      trauma = Mathf.Min(trauma + amount, 1.0f);
    }

    public override void _Process(float delta)
    {
      base._Process(delta);

      if (!(trauma > 0)) return;
    
      trauma = Mathf.Max(trauma - decay * delta, 0f);
      Shake();
    }

    private void Shake()
    {
      var amount = Mathf.Pow(trauma, traumaPower);
    
      noiseY++;
      Rotation = maxRoll * amount * noise.GetNoise2d(noise.Seed, noiseY);
      var offset = Offset;
      offset.x = maxOffset.x * amount * noise.GetNoise2d(noise.Seed * 2, noiseY);
      offset.y = maxOffset.y * amount * noise.GetNoise2d(noise.Seed * 3, noiseY);
      Offset = offset;
    }
  }
}
