using LiquidSnake.Enemies;
using UnityEngine;

public class Condition
{
    public virtual bool IsTrue()
    {
        return true;
    }
}

public class NestorDetected : Condition
{
    private VisionSensor visionSensor;
    LayerMask layerPlayer;
    private GameObject pinkRobot;

    public NestorDetected(StateMachineManager manager)
    {
        pinkRobot = manager.GetRobot();
        visionSensor = pinkRobot.GetComponentInChildren<VisionSensor>();

        layerPlayer = LayerMask.GetMask("Player");
    }


    public override bool IsTrue()
    {
        return visionSensor.GetClosestTarget().layer == layerPlayer;
    }
}

public class BulletsEmpty : Condition
{
    private LaserShooter shooterBullets;
    private GameObject pinkRobot;
    public BulletsEmpty(StateMachineManager manager)
    {
        pinkRobot = manager.GetRobot();
        shooterBullets = pinkRobot.GetComponent<LaserShooter>();
    }

    public override bool IsTrue()
    {
        return shooterBullets.BulletsEmpty();
    }
}

public class ReachBase : Condition
{
    SearchForBase reachBase;
    public ReachBase(StateMachineManager manager)
    {
        reachBase = manager.GetSearchForBase();
    }
    public override bool IsTrue()
    {
        return reachBase.isBaseReached();
    }
}

public class ReachNearestWaypoint : Condition
{
    SearchforNearestWaypoint reachNearestWaypoint;
    public ReachNearestWaypoint(StateMachineManager manager)
    {
        reachNearestWaypoint = manager.GetSearchForNearestWaypoint();
    }
    public override bool IsTrue()
    {
        return reachNearestWaypoint.isWaypointReached();
    }
}

public class AndCondition : Condition
{
    Condition condicionA;
    Condition condicionB;
    public AndCondition(Condition A, Condition B)
    {
        condicionA = A;
        condicionB = B;
    }

    public override bool IsTrue()
    {
        return condicionA.IsTrue() && condicionB.IsTrue();
    }
}

public class NotCondition : Condition
{
    Condition condicionNot;
    public NotCondition(Condition not)
    {
        condicionNot = not;
    }

    public override bool IsTrue()
    {
        return !condicionNot.IsTrue();
    }
}