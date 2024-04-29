using LiquidSnake.Enemies;
using LiquidSnake.Utils;
using UnityEngine;
using UnityEngine.AI;

public interface IAction : IResetteable
{
    // Update is called once per frame
    void Update();
}


public class DoPatrol : IAction
{
    GameObject currentWaypoint;
    GameObject targetWaypoint;
    bool inPatrol, backInPatrol;

    private RotateRandomTimes randomTurn;

    private NavMeshAgent navAgent;

    public DoPatrol() { }


    public void InitPatrol(GameObject current_waypoint, UnityEngine.AI.NavMeshAgent navMesh, RotateRandomTimes turn) { 
        
        currentWaypoint = current_waypoint;
        targetWaypoint = currentWaypoint.GetComponent<Waypoint>().nextWaypoint;
        inPatrol = true;
        backInPatrol = false;
        navAgent = navMesh;
        randomTurn = turn;

        navAgent.SetDestination(targetWaypoint.transform.position);
        //Coge el waypoint inicial y el target
        
     }

    public void Update()
    {
        if (inPatrol)
        {
            if (backInPatrol)
            {
                targetWaypoint = targetWaypoint.GetComponent<Waypoint>().nextWaypoint;
                backInPatrol = false;
            }
            if (navAgent.destination != targetWaypoint.transform.position)
                navAgent.SetDestination(targetWaypoint.transform.position);
            else if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                inPatrol = false;
                randomTurn.setIsTurning(true);

            }

        }
    }
    public void setPatrulla(bool isPatrulla)
    {
        inPatrol = isPatrulla;
    }

    public void setVuelve(bool isVuelve)
    {
        backInPatrol = isVuelve;
    }

    public void setCurrentTargetWaypoint(GameObject newWaypoint)
    {
        currentWaypoint = newWaypoint;
        targetWaypoint = newWaypoint.GetComponent<Waypoint>().nextWaypoint;
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }
    //en el update
    //bool esta en patrulla
    //bool vuelve
    //si vuelve de patrullar establece el target como target = target.nextwaypoint y vuelve false
    //si tiene que patrullar nawmesh.setdestination(target transform)

    //si llega al destino patrulla false y setea true el girar random

    //necesita
    //un setter de patrulla y de vuelve publicos
    //un setter de current waypoint
    //referencia a girar random
    //el navmeshagent
}

public class RotateRandomTimes: IAction
{
    //si rotar random true, segun x veces de rotaci�n se hace la acci�n de rotar 
    //una vez se hayan hecho las rotaciones se rotar random false y patrol true y vuelve true
    GameObject pinkRobot;
    DoPatrol patrol;
    bool rotateRandom, calculatedGoal;
    int timesRotated, maxRotations;
    private float rotationSpeed;
    private float _acceptableRange = 0.1f;
    private Quaternion _goalRotation;
    //necesito 
    //referencia a la accion de patrol para hacer los set a patrulla y vuelve
    public RotateRandomTimes() { }

    public void InitRotate(GameObject robopink, DoPatrol patrulla, float rotateSpeed)
    {
        pinkRobot = robopink;
        patrol = patrulla;
        rotationSpeed = rotateSpeed;
        rotateRandom = false;
        calculatedGoal = false;
        maxRotations = 5;
        timesRotated = 0;
    }

    public void Update()
    {
        if (rotateRandom)
        {   
            if (!calculatedGoal)
            {
                int direction = Random.Range(-1, 1) >= 0 ? 1 : -1;
                // nos aseguramos de que siempre haya una rotaci�n lo suficientemente amplia
                _goalRotation = Quaternion.Euler(0, Random.Range(90f, 180f) * direction, 0);
                calculatedGoal = true;
            }
            float currentAngularDistance = Quaternion.Angle(pinkRobot.transform.rotation, _goalRotation);


            float t = Mathf.Clamp01(Time.deltaTime * rotationSpeed / currentAngularDistance);

            pinkRobot.transform.rotation =
                Quaternion.Slerp(pinkRobot.transform.rotation,
                _goalRotation, t);

            var dot = Quaternion.Dot(_goalRotation, pinkRobot.transform.rotation);
            var abs = Mathf.Abs(dot);
            if (1 - abs <= _acceptableRange)
            {
                calculatedGoal = false;
                if(++timesRotated >= maxRotations)
                {
                    rotateRandom = false;
                    patrol.setPatrulla(true);
                    patrol.setVuelve(true);
                }

            }
        }
    }
    public void setIsTurning(bool isTurning)
    {
        rotateRandom = isTurning;  
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }
}
public class RobotHunt : IAction
{
    private GameObject nestor;
    private GameObject pinkRobot;
    float punteria;
    private float chaseSpeed = 3.0f;
    private NavMeshAgent _navMeshAgent;
    private float timeUntilNextShoot = 0.5f;
    private float cooldown = 0.5f;
    private LaserShooter shooter;

    public RobotHunt() {}


    public void InitRobotHunt(GameObject nestorGO, NavMeshAgent navegacion, GameObject roboPink)
    {
        pinkRobot = roboPink;
        nestor = nestorGO;
        _navMeshAgent = navegacion;
        _navMeshAgent.speed = chaseSpeed;
        _navMeshAgent.SetDestination(nestor.transform.position);
        shooter = pinkRobot.GetComponent<LaserShooter>();
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        timeUntilNextShoot -= Time.deltaTime;
        timeUntilNextShoot = Mathf.Max(0f, timeUntilNextShoot);

        if (timeUntilNextShoot <= 0f)
        {
            shooter.Shoot(nestor.transform, punteria);

            timeUntilNextShoot = cooldown;
        }
        _navMeshAgent.SetDestination(nestor.transform.position);
    }
    //Si estas en caza,
    //navmesh.setdestination(transform de nestor)
    //disparas cada x segundos

    //necesitas
    //setter la precisi�n la punter�a consiste en aplicarle un offset a la posici�n de disparo seg�n la precisi�n de disparo
    //el transform de nestor
    //un navmesh agent
    //las variables para el timer (referirse a lase shooter)
    public void SetAccuracy(float newAccuracy)
    {
        punteria = newAccuracy;
    }


}

public class RecalculateAim : IAction
{
    private float timeUntilCalculation = 1f;
    private float cooldown = 1f;
    //cada x tiempo se recalcula la precision en funcion de la distancia a la que esta el robot de nestor.
    RobotHunt robotHunt;
    GameObject pinkRobot, nestorRobot;
    float minDist, maxDist;
    //necesita
    //una referencia a la acci�n de robothunt para settear la accuracy
    //El gameobject del robot y de nestor
    //las variables del timer(referirse a laser shooter)
    public RecalculateAim(){ }

    public void InitAim(RobotHunt roboHunt, GameObject roboPink, GameObject nestor, float distMin, float distMax) { 
        robotHunt = roboHunt;
        pinkRobot = roboPink;
        nestorRobot = nestor;
        minDist = distMin;
        maxDist = distMax;
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        timeUntilCalculation -= Time.deltaTime;
        timeUntilCalculation = Mathf.Max(0f, timeUntilCalculation);

        if(timeUntilCalculation <= 0f)
        {
            timeUntilCalculation = cooldown;
            float distance = Vector3.Distance(pinkRobot.transform.position, nestorRobot.transform.position);
            distance = Mathf.Clamp(distance, minDist, maxDist);
            float accuracy = Mathf.Lerp(1, 0, Mathf.InverseLerp(minDist, maxDist, distance));
            // robotHunt.setAccuracy(accuracy);
        }
    }

}


public class SearchForBase : IAction
{
    //si busca la base, se hace navmeshagent.setdestination(base.transform)
    //y le dices al patrol cual es su current waypoint
    GameObject baseWaypoint;
    DoPatrol patrulla;
    private NavMeshAgent navAgent;
    bool baseReached;
    //necesita 
    //navmeshagent
    //el GO waypoint o su transform
    //una referencia a la accion de patrulla para settear el current waypoiont de la patrulla
    public SearchForBase(){}

    public void InitSearchBase(GameObject target, DoPatrol patrol, UnityEngine.AI.NavMeshAgent navMesh )
    {
        baseReached = false;
        baseWaypoint = target;
        patrulla = patrol;
        navAgent = navMesh;
        navAgent.SetDestination(baseWaypoint.transform.position);
    }

    public void Update()
    {
        if (navAgent.destination != baseWaypoint.transform.position)
            navAgent.SetDestination(baseWaypoint.transform.position);
        else if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            patrulla.setCurrentTargetWaypoint(baseWaypoint);
            baseReached = true;
        }
    }

    public bool isBaseReached()
    {
        return baseReached;
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }
}

public class SearchforNearestWaypoint : IAction
{
    int waypointQuantity;
    GameObject target;
    GameObject pinkRobot;
    private NavMeshAgent navAgent;
    DoPatrol patrulla;
    private bool targetCalculated;
    private bool waypointReached;

    //necesitas
    //nuimero de waypoints
    //waypoint base y su posici�n
    //navmeshagent
    //referencia al accion patrulla para cambiar el current waypoint

    public SearchforNearestWaypoint(){}

    public void InitNearestWaypoint(int nWaypoints, GameObject objetivo, UnityEngine.AI.NavMeshAgent navMesh, DoPatrol patrol, GameObject roboPink)
    {
        waypointQuantity = nWaypoints;
        target = objetivo;
        navAgent = navMesh;
        patrulla = patrol;
        targetCalculated = false;
        pinkRobot = roboPink;
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }

    //si busca waypoint mas cercano, buscas entre los x waypoints de tu ruta(lo especifica el state machine)
    //vas de uno en uno calculando la distancia y cuando llegues al final el waypoint mas cercano es el que tienes que hacer
    //navmeshagent.setdestination(waypoint mas cercano.position) y le dices al patro cual es su current waypoint
    public void Update()
    {
        if (!targetCalculated)
        {
            for(int i = 0; i < waypointQuantity; i++)
            {
                float minDistance = Vector3.Distance(pinkRobot.transform.position, target.transform.position);
                float minDistanceCandidate = Vector3.Distance(pinkRobot.transform.position, target.GetComponent<Waypoint>().nextWaypoint.transform.position);
                if(minDistanceCandidate <= minDistance)
                {
                    target = target.GetComponent<Waypoint>().nextWaypoint;
                }
            }
            targetCalculated = true;
            navAgent.SetDestination(target.transform.position); 
        }
        if (navAgent.destination != target.transform.position)
            navAgent.SetDestination(target.transform.position);
        else if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            patrulla.setCurrentTargetWaypoint(target);
            waypointReached = true;
        }
    }

    public bool isWaypointReached()
    {
        return waypointReached;
    }
}

public class ReloadBullets : IAction
{
    private float timeUntilReload = 4f;
    private float cooldown = 4f;
    //cada x tiempo se recalcula la precision en funcion de la distancia a la que esta el robot de nestor.
    GameObject pinkRobot;
    //necesita
    //una referencia a la acci�n de robothunt para settear la accuracy
    //El gameobject del robot y de nestor
    //las variables del timer(referirse a laser shooter)
    public ReloadBullets() { }

    public void InitReload(GameObject roboPink)
    {
        pinkRobot = roboPink;
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        timeUntilReload -= Time.deltaTime;
        timeUntilReload = Mathf.Max(0f, timeUntilReload);

        if (timeUntilReload <= 0f)
        {
            timeUntilReload = cooldown;
            pinkRobot.GetComponent<LaserShooter>().rechargeBullets();
        }
    }

}