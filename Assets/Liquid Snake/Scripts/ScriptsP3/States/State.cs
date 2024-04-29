using System.Collections.Generic;

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
    public Patrol() { }
    public void InitPatrol(StateMachineManager manager)
    {
        transitions = new List<Transition>();
        whileActions = new List<IAction>();

        FollowShoot patrolFollow = manager.GetFollowShoot();
        Transition transition1 = new Transition(new NestorDetected(manager), patrolFollow);

        DoPatrol doPatrol = manager.GetDoPatrol();
        RotateRandomTimes patrolRotate = manager.GetRotateRandom();
        
        transitions.Add(transition1); // Patrulla a persecución con disparo
        whileActions.Add(doPatrol);
        whileActions.Add(patrolRotate);
    }
}

public class FollowShoot : State
{
    public FollowShoot() { }

    public void InitFollowShoot(StateMachineManager manager) 
    {
        transitions = new List<Transition>();
        whileActions = new List<IAction>();

        NotCondition condicionPatrol = new NotCondition(new NestorDetected(manager));
        GoToNearestWaypoint nearestWaypoint = manager.GetNearestWaypoint();
        Transition transition1 = new Transition(condicionPatrol, nearestWaypoint);
        GoToBase baseFollowShoot = manager.GetGoToBase();
        Transition transition2 = new Transition(new BulletsEmpty(manager), baseFollowShoot);

        RobotHunt robotHunt = manager.GetRobotHunt();
        RecalculateAim recalculateAim = manager.GetAimRecalculation();

        transitions.Add(transition1); // Persecución a ruta de patrulla
        transitions.Add(transition2); // Persecución a la base
        whileActions.Add(robotHunt);
        whileActions.Add(recalculateAim);
    }
}

public class GoToNearestWaypoint : State
{
    public GoToNearestWaypoint() { }

    public void InitGoToNearestWaypoint(StateMachineManager manager)
    {
        transitions = new List<Transition>();
        whileActions = new List<IAction>();

        Patrol patrol = manager.GetPatrol();
        Transition transition1 = new Transition(new ReachNearestWaypoint(manager), patrol);
        FollowShoot followShoot = manager.GetFollowShoot();
        Transition transition2 = new Transition(new NestorDetected(manager), followShoot);

        SearchforNearestWaypoint searchforNearestWaypoint = manager.GetSearchForNearestWaypoint();
            
        transitions.Add(transition1); // Ruta de patrulla a la propia patrulla
        transitions.Add(transition2);
        whileActions.Add(searchforNearestWaypoint);
    }
}

public class GoToBase : State
{
    public GoToBase() { }

    public void InitGoToBase(StateMachineManager manager)
    {
        transitions = new List<Transition>();
        whileActions = new List<IAction>();

        Reload reload = manager.GetReload();
        Transition transition1 = new Transition(new ReachBase(manager), reload);
        FollowShoot followShoot = manager.GetFollowShoot();
        Transition transition2 = new Transition(new NestorDetected(manager), followShoot);

        SearchForBase searchForBase = manager.GetSearchForBase();

        transitions.Add(transition1); // Base a recargar
        transitions.Add(transition2); // Base a recargar
        whileActions.Add(searchForBase);
    }
}

public class Reload : State
{
    public Reload() { }

    public void InitReload(StateMachineManager manager)
    {
        transitions = new List<Transition>();
        whileActions = new List<IAction>();

        AndCondition condicionFollowShoot = new AndCondition(new NestorDetected(manager), new NotCondition(new BulletsEmpty(manager)));
        AndCondition condicionPatrol = new AndCondition(new NotCondition(new NestorDetected(manager)), new NotCondition(new BulletsEmpty(manager)));
        FollowShoot followShoot = manager.GetFollowShoot();
        Transition transition1 = new Transition(condicionFollowShoot, followShoot);
        Patrol patrol = manager.GetPatrol();
        Transition transition2 = new Transition(condicionPatrol, patrol);
            
        ReloadBullets reloadBullets = manager.GetReloadBullets();

        transitions.Add(transition1); // Recarga a persecución
        transitions.Add(transition2); // Recarga a patrulla
        whileActions.Add(reloadBullets);
    }
}