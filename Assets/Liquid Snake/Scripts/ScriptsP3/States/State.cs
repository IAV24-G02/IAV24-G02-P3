using LiquidSnake.Enemies;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected List<IAction> WhileActions;
    protected List<IAction> ExitActions;
    protected List<IAction> EntryActions;
    protected List<Transition> transitions;



    public List<IAction> GetWhileActions()
    {
        return WhileActions;
    }

    public List<IAction> GetExitActions()
    {
        return ExitActions;
    }

    public List<IAction> GetEntryActions()
    {
        return EntryActions;
    }

    public List<Transition> GetTransitions()
    {
        return transitions;
    }
}

public class Patrol : State
{
    public Patrol(GameObject initialWaypoint)
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

public class GoToNearestBase : State
{
    public GoToNearestBase()
    {

    }
}

public class Reload : State
{
    public Reload()
    {

    }
}