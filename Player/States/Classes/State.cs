using Godot;
using System;
using System.Collections.Generic;

public partial class State : Node
{   
    [Signal]
    public delegate void FinishedEventHandler(String targetStatePath);

    public virtual void Enter(string previousStatePath) { //do I need this still?

    }
    public virtual void Exit() { 

    }
    public virtual void HandleInput(InputEvent evt) { 

    }
    public virtual void Update(double delta) { 

    }
    public virtual void PhysicsUpdate(double delta) { 

    }
}
