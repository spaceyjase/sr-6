using System;

namespace StateMachine
{
  public class State
  {
    public string Name { get; set; }
    public Action OnProcess { get; set; }
    public Action OnPhysicsProcess { get; set; }
    public Action OnEnter { get; set; }
    public Action OnExit { get; set; }

    public override string ToString()
    {
      return Name;
    }
  }
}