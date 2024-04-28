using LiquidSnake.Enemies;
using UnityEngine;

public class Condition
{
    protected GameObject pinkRobot;

    public virtual bool IsTrue()
    {
        return true;
    }
}

public class NestorDetected : Condition
{
    private VisionSensor visionSensor;
    LayerMask layerPlayer;

    public NestorDetected(GameObject robot)
    {
        pinkRobot = robot;
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
    public BulletsEmpty(GameObject robot)
    {
        pinkRobot = robot;
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
    public ReachBase(SearchForBase llega)
    {
        reachBase = llega;
    }
    public override bool IsTrue()
    {
        return reachBase.isBaseReached();
    }
}

public class ReachNearestWaypoint : Condition
{
    SearchforNearestWaypoint reachNearestWaypoint;
    public ReachNearestWaypoint(SearchforNearestWaypoint reachWaypoint)
    {
        reachNearestWaypoint = reachWaypoint;
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