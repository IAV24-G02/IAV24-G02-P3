using System.Collections.Generic;

public class Transition
{
    private List<IAction> actions;
    private State targetState;
    private Condition condition;

    public Transition(Condition newCondition, State newState, List<IAction> newActions = null)
    {
        condition = newCondition;
        targetState = newState;
        actions = newActions ?? new List<IAction>();
    }


    public List<IAction> GetActions()
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