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

    NestorDetected(GameObject robot)
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

public class ReloadBullets : Condition
{
    private LaserShooter shooterBullets;
    ReloadBullets(GameObject robot)
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
    ReachBase(SearchForBase llega)
    {
        reachBase = llega;
    }
    public override bool IsTrue()
    {
        return reachBase.isBaseReached();
    }
}