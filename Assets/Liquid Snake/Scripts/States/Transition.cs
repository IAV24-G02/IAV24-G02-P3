using System.Collections.Generic;

public class Transition
{
    private List<Action> actions;
    private State targetState;
    private Condition condition;

    public List<Action> GetActions()
    {
        return actions;
    }

    public State GetTargetState()
    {
        return targetState;
    }

    public bool IsTriggered()
    {
        return condition.IsTrue();
    }
}
