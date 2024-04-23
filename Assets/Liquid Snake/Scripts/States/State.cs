using System.Collections.Generic;

public class State
{
    protected List<Action> actions;
    protected List<Transition> transitions;


    public List<Action> GetActions()
    {
        return actions;
    }

    public List<Transition> GetTransitions()
    {
        return transitions;
    }
}

public class Patrol : State
{
    public Patrol()
    {
        // entryActions.Add()
    }
}

public class FollowShoot : State
{
    public FollowShoot()
    {

    }
}

public class GoToNearestWaypoint : State
{
    public GoToNearestWaypoint()
    {

    }
}

public class Reload : State
{
    public Reload()
    {

    }
}

public class RecalculateAim : State
{
    public RecalculateAim()
    {

    }
}
