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


    public void initPatrol(GameObject current_waypoint, UnityEngine.AI.NavMeshAgent navMesh, RotateRandomTimes turn) { 
        
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
    //si rotar random true, segun x veces de rotación se hace la acción de rotar 
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

    public void initRotate(GameObject robopink, DoPatrol patrulla, float rotateSpeed)
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
                // nos aseguramos de que siempre haya una rotación lo suficientemente amplia
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
    float punteria;
    private float chaseSpeed = 3.0f;
    private NavMeshAgent _navMeshAgent;


    public RobotHunt() {}


    public void initRObotHunt(GameObject nestorGO, NavMeshAgent navegacion)
    {
        nestor = nestorGO;
        _navMeshAgent = navegacion;
        _navMeshAgent.speed = chaseSpeed;
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        _navMeshAgent.SetDestination(nestor.transform.position);
    }
    //Si estas en caza,
    //navmesh.setdestination(transform de nestor)
    //disparas cada x segundos

    //necesitas
    //setter la precisión la puntería consiste en aplicarle un offset a la posición de disparo según la precisión de disparo
    //el transform de nestor
    //un navmesh agent
    //las variables para el timer (referirse a lase shooter)
    public void setAccuracy(float newAccuracy)
    {
        punteria = newAccuracy;
    }


}

public class RecalculateAim : IAction
{
    private float _timeUntilNextShoot = 0.8f;
    //cada x tiempo se recalcula la precision en funcion de la distancia a la que esta el robot de nestor.
    RobotHunt robotHunt;
    GameObject pinkRobot, nestorRobot;
    float minDist, maxDist;
    //necesita
    //una referencia a la acción de robothunt para settear la accuracy
    //El gameobject del robot y de nestor
    //las variables del timer(referirse a laser shooter)
    public RecalculateAim(){ }

    public void initAim(RobotHunt roboHunt, GameObject roboPink, GameObject nestor, float distMin, float distMax) { 
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
        _timeUntilNextShoot -= Time.deltaTime;
        _timeUntilNextShoot = Mathf.Max(0f, _timeUntilNextShoot);

        if( _timeUntilNextShoot <= 0f)
        {
            _timeUntilNextShoot = 1f;
            float distance = Vector3.Distance(pinkRobot.transform.position, nestorRobot.transform.position);
            distance = Mathf.Clamp(distance, minDist, maxDist);
            float accuracy = Mathf.Lerp(1, 0, Mathf.InverseLerp(minDist, maxDist, distance));
            robotHunt.setAccuracy(accuracy);
        }
    }

}


public class SearchForBase : IAction
{
    //si busca la base, se hace navmeshagent.setdestination(base.transform)
    //y le dices al patrol cual es su current waypoint
    GameObject baseWaypoint;
    DoPatrol patrulla;
    private UnityEngine.AI.NavMeshAgent navAgent;
    bool baseReached;
    //necesita 
    //navmeshagent
    //el GO waypoint o su transform
    //una referencia a la accion de patrulla para settear el current waypoiont de la patrulla
    public SearchForBase(){}

    public void initSearchBase(GameObject target, DoPatrol patrol, UnityEngine.AI.NavMeshAgent navMesh )
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
    //si busca waypoint mas cercano, buscas entre los x waypoints de tu ruta(lo especifica el state machine)
    //vas de uno en uno calculando la distancia y cuando llegues al final el waypoint mas cercano es el que tienes que hacer
    //navmeshagent.setdestination(waypoint mas cercano.position) y le dices al patro cual es su current waypoint
    int waypointQuantity;
    GameObject target;
    private UnityEngine.AI.NavMeshAgent navAgent;
    DoPatrol patrulla;

    //necesitas
    //nuimero de waypoints
    //waypoint base y su posición
    //navmeshagent
    //referencia al accion patrulla para cambiar el current waypoint7

    public SearchforNearestWaypoint(){}

    public void initNearestWaypoint(int nWaypoints, GameObject objetivo, UnityEngine.AI.NavMeshAgent navMesh, DoPatrol patrol)
    {
        waypointQuantity = nWaypoints;
        target = objetivo;
        navAgent = navMesh;
        patrulla = patrol;
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {

    }
}
