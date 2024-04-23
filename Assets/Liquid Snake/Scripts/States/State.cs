using System.Collections.Generic;

public class State
{
    private List<Action> entryActions;
    private List<Action> actions;
    private List<Action> exitActions;
    private List<Transition> transitions;


    public List<Action> GetActions()
    {
        return actions;
    }
    public List<Action> GetEntryActions()
    {
        return entryActions;
    }
    public List<Action> GetExitActions()
    {
        return exitActions;
    }

    public List<Transition> GetTransitions()
    {
        return transitions;
    }
}
