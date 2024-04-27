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
        transitions.Add(new Transition(new ReloadBullets(gameObject), new GoToNearestWaypoint(gameObject))); // Persecuci�n a la base
        transitions.Add(new Transition(new Condition(), new GoToNearestBase(gameObject))); // Persecuci�n a ruta de patrulla
    }
}

public class GoToNearestWaypoint : State
{
    public GoToNearestWaypoint(GameObject gameObject)
    {
        transitions.Add(new Transition(new Condition(), new Patrol(gameObject))); // Ruta de patrulla a la propia patrulla
    }
}

public class GoToNearestBase : State
{
    public GoToNearestBase(GameObject gameObject)
    {
        transitions.Add(new Transition(new Condition(), new Reload(gameObject))); // Base a recargar
    }
}

public class Reload : State
{
    public Reload(GameObject gameObject)
    {
        transitions.Add(new Transition(new Condition(), new FollowShoot(gameObject))); // Recarga a persecuci�n
        transitions.Add(new Transition(new Condition(), new Patrol(gameObject))); // Recarga a patrulla
    }
}