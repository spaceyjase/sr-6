using System.Collections.Generic;
using Godot;

namespace StateMachine
{
  public class StateMachine : Node
  {
    private readonly Dictionary<string, State> states = new Dictionary<string, State>();

    private State CurrentState { get; set; }
    private State InitialState { get; set; }

    public override void _Ready()
    {
      base._Ready();

      foreach (var child in GetChildren())
      {
        if (!(child is State state)) continue;
        
        if (states.Count == 0)
        {
          InitialState = state;
        }
        states[state.Name] = state;
      }
    }

    public override void _Process(float delta)
    {
      base._Process(delta);

      if (states.Count == 0 || InitialState == null)
      {
        GD.PrintErr("State machine has no states!");
        return;
      }

      if (CurrentState == null)
      {
        ChangeState(InitialState);
      }

      CurrentState.OnProcess?.Invoke();
    }

    public override void _PhysicsProcess(float delta)
    {
      base._PhysicsProcess(delta);
      
      if (states.Count == 0 || InitialState == null)
      {
        GD.PrintErr("State machine has no states!");
        return;
      }

      if (CurrentState == null)
      {
        ChangeState(InitialState);
      }

      CurrentState.OnPhysicsProcess?.Invoke();
    }

    private void ChangeState(State newState)
    {
      if (newState == null)
      {
        GD.PrintErr("Cannot transition to a null state!");
        return;
      }

      CurrentState?.OnExit?.Invoke();
      
      GD.Print($"Transitioning from {CurrentState} to {newState}");

      CurrentState = newState;
      
      newState.OnEnter?.Invoke();
    }
    
    private void ChangeState(string name)
    {
      if (states.ContainsKey(name) == false)
      {
        GD.PrintErr($"State {name} does not exist!");
        return;
      }

      ChangeState(states[name]);
    }
  }
}