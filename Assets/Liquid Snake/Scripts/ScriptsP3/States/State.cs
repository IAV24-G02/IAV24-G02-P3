using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    protected List<IAction> whileActions;
    protected List<IAction> exitActions;
    protected List<IAction> entryActions;
    protected List<Transition> transitions;

    public List<IAction> GetWhileActions()
    {
        return whileActions;
    }

    public List<IAction> GetExitActions()
    {
        return exitActions;
    }

    public List<IAction> GetEntryActions()
    {
        return entryActions;
    }

    public List<Transition> GetTransitions()
    {
        return transitions;
    }
}

public class Patrol : State
{
    public Patrol(StateMachineManager manager)
    {
        transitions.Add(new Transition(new NestorDetected(manager), manager.GetFollowShoot())); // Patrulla a persecución con disparo
        whileActions.Add(manager.GetDoPatrol());
        whileActions.Add(manager.GetRotateRandom());
    }
}

public class FollowShoot : State
{
    public FollowShoot(StateMachineManager manager)
    {
        NotCondition condicionPatrol = new NotCondition(new NestorDetected(manager));
        transitions.Add(new Transition(new BulletsEmpty(manager), manager.GetGoToBase())); // Persecución a la base
        transitions.Add(new Transition(condicionPatrol, manager.GetNearestWaypoint())); // Persecución a ruta de patrulla
        whileActions.Add(manager.GetRobotHunt());
        whileActions.Add(manager.GetAimRecalculation());
    }
}

public class GoToNearestWaypoint : State
{
    public GoToNearestWaypoint(StateMachineManager manager)
    {
        transitions.Add(new Transition(new ReachNearestWaypoint(manager), manager.GetPatrol())); // Ruta de patrulla a la propia patrulla
        transitions.Add(new Transition(new NestorDetected(manager), manager.GetFollowShoot()));
        whileActions.Add(manager.GetSearchForNearestWaypoint());
    }
}

public class GoToBase : State
{
    public GoToBase(StateMachineManager manager)
    {
        transitions.Add(new Transition(new ReachBase(manager), manager.GetReload())); // Base a recargar
        transitions.Add(new Transition(new NestorDetected(manager), manager.GetFollowShoot())); // Base a recargar
        whileActions.Add(manager.GetSearchForBase());
    }
}

public class Reload : State
{
    public Reload(StateMachineManager manager)
    {
        AndCondition condicionFollowShoot = new AndCondition(new NestorDetected(manager), new NotCondition(new BulletsEmpty(manager)));
        AndCondition condicionPatrol = new AndCondition(new NotCondition(new NestorDetected(manager)), new NotCondition(new BulletsEmpty(manager)));
        transitions.Add(new Transition(condicionFollowShoot, manager.GetFollowShoot())); // Recarga a persecución
        transitions.Add(new Transition(condicionPatrol, manager.GetPatrol())); // Recarga a patrulla
        whileActions.Add(manager.GetReloadBullets());
    }
}