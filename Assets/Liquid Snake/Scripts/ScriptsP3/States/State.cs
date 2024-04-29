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
    DoPatrol patrolAction;
    RotateRandomTimes rotateAction;
    public Patrol(GameObject gameObject)
    {
        patrolAction = new DoPatrol();
        rotateAction = new RotateRandomTimes();

        
        transitions.Add(new Transition(new NestorDetected(gameObject), new FollowShoot(gameObject))); // Patrulla a persecución con disparo
    }
}

public class FollowShoot : State
{
    RobotHunt followAction;
    RecalculateAim recalculateAction;
    public FollowShoot(GameObject gameObject)
    {
        followAction = new RobotHunt();
        recalculateAction = new RecalculateAim();

        NotCondition condicionPatrol = new NotCondition(new NestorDetected(gameObject));
        transitions.Add(new Transition(new BulletsEmpty(gameObject), new GoToBase(gameObject))); // Persecución a la base
        transitions.Add(new Transition(condicionPatrol, new GoToNearestWaypoint(gameObject))); // Persecución a ruta de patrulla
    }
}

public class GoToNearestWaypoint : State
{
    SearchforNearestWaypoint searchAction;
    public GoToNearestWaypoint(GameObject gameObject)
    {
        searchAction = new SearchforNearestWaypoint();

        transitions.Add(new Transition(new ReachNearestWaypoint(searchAction), new Patrol(gameObject))); // Ruta de patrulla a la propia patrulla
        //transitions.Add(new Transition(new NestorDetected(gameObject), new FollowShoot(gameObject));
    }
}

public class GoToBase : State
{
    SearchForBase searchAction;
    public GoToBase(GameObject gameObject)
    {
        searchAction = new SearchForBase();
        transitions.Add(new Transition(new ReachBase(searchAction), new Reload(gameObject))); // Base a recargar
    }
}

public class Reload : State
{
    ReloadBullets reloadingAction;
    public Reload(GameObject gameObject)
    {
        reloadingAction = new ReloadBullets(); 
        AndCondition condicionFollowShoot = new AndCondition(new NestorDetected(gameObject), new NotCondition(new BulletsEmpty(gameObject)));
        AndCondition condicionPatrol = new AndCondition(new NotCondition(new NestorDetected(gameObject)), new NotCondition(new BulletsEmpty(gameObject)));
        transitions.Add(new Transition(condicionFollowShoot, new FollowShoot(gameObject))); // Recarga a persecución
        transitions.Add(new Transition(condicionPatrol, new Patrol(gameObject))); // Recarga a patrulla
    }
}