using System.Collections.Generic;
using UnityEngine;

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
    public Patrol(GameObject gameObject)
    {   

        transitions.Add(new Transition(new NestorDetected(gameObject), new FollowShoot(gameObject))); // Patrulla a persecuci�n con disparo
    }
}

public class FollowShoot : State
{
    public FollowShoot(GameObject gameObject)
    {
        NotCondition condicionPatrol = new NotCondition(new NestorDetected(gameObject));
        transitions.Add(new Transition(new BulletsEmpty(gameObject), new GoToBase(gameObject))); // Persecuci�n a la base
        transitions.Add(new Transition(condicionPatrol, new GoToNearestWaypoint(gameObject))); // Persecuci�n a ruta de patrulla
    }
}

public class GoToNearestWaypoint : State
{
    public GoToNearestWaypoint(GameObject gameObject)
    {
        transitions.Add(new Transition(new ReachNearestWaypoint(gameObject), new Patrol(gameObject))); // Ruta de patrulla a la propia patrulla
        //transitions.Add(new Transition(new NestorDetected(gameObject), new FollowShoot(gameObject));
    }
}

public class GoToBase : State
{
    public GoToBase(GameObject gameObject)
    {
        transitions.Add(new Transition(new ReachBase(gameObject), new Reload(gameObject))); // Base a recargar
    }
}

public class Reload : State
{
    public Reload(GameObject gameObject)
    {
        AndCondition condicionFollowShoot = new AndCondition(new NestorDetected(gameObject), new NotCondition(new BulletsEmpty(gameObject)));
        AndCondition condicionPatrol = new AndCondition(new NotCondition(new NestorDetected(gameObject)), new NotCondition(new BulletsEmpty(gameObject)));
        transitions.Add(new Transition(condicionFollowShoot, new FollowShoot(gameObject))); // Recarga a persecuci�n
        transitions.Add(new Transition(condicionPatrol, new Patrol(gameObject))); // Recarga a patrulla
    }
}