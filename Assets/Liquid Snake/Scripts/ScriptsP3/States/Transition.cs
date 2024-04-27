using System.Collections.Generic;

public class Transition
{
    private List<IAction> actions;
    private State targetState;
    private Condition condition;


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

public class Out_Patrol : Transition
{

}

public class Out_Follow_Shoot : Transition
{

}

public class Out_Go_To_Base : Transition
{

}

public class Out_Go_To_Nearest_Waypoint : Transition
{

}

public class Out_Reload : Transition
{

}