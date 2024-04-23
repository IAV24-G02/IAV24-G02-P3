using LiquidSnake.Enemies;
using UnityEngine;

public class Condition : MonoBehaviour
{
    protected GameObject nestor;

    public virtual bool IsTrue()
    {
        return true;
    }
}

public class NestorDetected : Condition
{
    private VisionSensor visionSensor;
    LayerMask layerPlayer;

    void Start()
    {
        nestor = transform.gameObject;
        Debug.Log(nestor);
        visionSensor = nestor.GetComponent<VisionSensor>();

        layerPlayer = LayerMask.GetMask("Player");
    }


    public override bool IsTrue()
    {
        return visionSensor.GetClosestTarget().layer == layerPlayer; 
    }
}

public class BulletsReloaded : Condition
{
    void Start()
    {
        
    }

    public override bool IsTrue()
    {
        return true;
    }
}