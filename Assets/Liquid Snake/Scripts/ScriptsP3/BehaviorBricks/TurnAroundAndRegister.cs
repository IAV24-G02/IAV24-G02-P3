using BBUnity.Actions;
using LiquidSnake.Enemies;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;

[Action("LiquidSnake/TurnAroundAndRegister")]
[Help("Smoothly rotates the character 360 degrees")]
public class TurnAroundAndRegister : GOAction
{
    [InParam("rotationSpeed")]
    [Help("Rotation speed")]
    public float rotationSpeed;

    [InParam("visionSensorObject")]
    [Help("visionSensorObject")]
    public GameObject visionSensor;

    private Quaternion _initialRotation;
    private float _currentRotation;
    private VisionSensor _visionSensor;
    private Register _register;

    public override void OnStart()
    {
        base.OnStart();
        _initialRotation = gameObject.transform.rotation;
        _currentRotation = 0f;
        visionSensor.SetActive(true);
        _visionSensor = visionSensor.GetComponent<VisionSensor>();
        if (_visionSensor == null)
        {
            Debug.LogError("Vision sensor not found");
        }
        else
        {
            _visionSensor.enabled = true;
        }
        _register = gameObject.GetComponent<Register>();
        if (_register == null)
        {
            Debug.LogError("Register not found");
        }
        else
        {
            _register.enabled = true;
        }
    }

    public override TaskStatus OnUpdate()
    {
        base.OnUpdate();

        GameObject closestTarget = _visionSensor.GetClosestTarget();
        if (closestTarget != null && _register != null)
        {
            if (closestTarget.CompareTag("SF_Door") && !_register.doors.Contains(closestTarget))
            {
                _register.doors.Add(closestTarget);
            }
            else if (closestTarget.CompareTag("Button") && !_register.buttons.Contains(closestTarget))
            {
                _register.buttons.Add(closestTarget);
            }
            else if (closestTarget.CompareTag("Checkpoint") && !_register.healthSpots.Contains(closestTarget))
            {
                _register.healthSpots.Add(closestTarget);
            }
            else if (closestTarget.CompareTag("Room") && !_register.rooms.Contains(closestTarget))
            {
                _register.rooms.Add(closestTarget);
            }
            else if (closestTarget.CompareTag("Goal") && _register.exit == null)
            {
                _register.exit = closestTarget;
            }
        }

        float rotationThisFrame = rotationSpeed * Time.deltaTime;
        _currentRotation += rotationThisFrame;

        if (_currentRotation > 360f)
        {
            _currentRotation = 360f;
        }

        gameObject.transform.rotation = _initialRotation * Quaternion.Euler(0, _currentRotation, 0);

        if (_currentRotation >= 360f)
        {
            return TaskStatus.COMPLETED;
        }

        return TaskStatus.RUNNING;
    }
}
