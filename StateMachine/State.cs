using System;
using Godot;

namespace StateMachine
{
  public class State : Node
  {
    public Action<float> OnProcess { get; set; }
    public Action<float> OnPhysicsProcess { get; set; }
    public Action OnEnter { get; set; }
    public Action OnExit { get; set; }
    public Action<InputEvent> OnInput { get; set; }

    public StateMachine StateMachine { get; set; }

    public override string ToString()
    {
      return Name;
    }
  }
}