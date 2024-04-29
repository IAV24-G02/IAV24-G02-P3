using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateMachineManager : MonoBehaviour
{
    #region Referencias a eltos. de la escena
    [SerializeField]
    private GameObject nestor;
    [SerializeField]
    private GameObject pinkRobot;
    [SerializeField]
    private GameObject initialWaypoint;
    [SerializeField]
    private GameObject currentWaypoint;
    [SerializeField]
    private NavMeshAgent navmesh;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float minDist;
    [SerializeField]
    private float maxDist;
    [SerializeField]
    private int numWaypoints;
    #endregion

    #region Getters de eltos. de la escena
    public GameObject GetNestor() { return nestor; }
    public GameObject GetRobot() { return pinkRobot; }
    public GameObject GetCurrentWaypoint() { return currentWaypoint; }
    public NavMeshAgent GetNavMeshAgent() { return navmesh; }
    public float GetRotationSpeed() { return rotationSpeed; }
    public float GetMinDistance() { return minDist; }
    public float GetMaxDistance() { return maxDist; }
    private int GetNumWaypoints() { return numWaypoints; }

    #endregion

    #region Estados
    private Patrol patrol;
    private FollowShoot followShoot;
    private GoToBase goToBase;
    private GoToNearestWaypoint goToWaypoint;
    private Reload reload;
    #endregion

    #region Getters de estados
    public Patrol GetPatrol() { return patrol; }
    public FollowShoot GetFollowShoot() { return followShoot; }
    public GoToBase GetGoToBase() { return goToBase; }
    public GoToNearestWaypoint GetNearestWaypoint() { return goToWaypoint; }
    public Reload GetReload() { return reload; }
    #endregion

    #region Acciones
    private DoPatrol doPatrol;
    private RotateRandomTimes rotateRandom;
    private RobotHunt robotHunt;
    private RecalculateAim recalculateAim;
    private SearchForBase searchBase;
    private SearchforNearestWaypoint searchWaypoint;
    private ReloadBullets reloadBullets;
    #endregion

    #region Getters de acciones
    public DoPatrol GetDoPatrol() { return doPatrol; }
    public RotateRandomTimes GetRotateRandom() { return rotateRandom; }
    public RobotHunt GetRobotHunt() { return robotHunt; }
    public RecalculateAim GetAimRecalculation() { return recalculateAim; }
    public SearchForBase GetSearchForBase() { return searchBase; }
    public SearchforNearestWaypoint GetSearchForNearestWaypoint() { return searchWaypoint; }
    public ReloadBullets GetReloadBullets() { return reloadBullets; }
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        // Creación de los estados a utilizar por el robot
        patrol = new Patrol(this);
        followShoot = new FollowShoot(this);
        goToBase = new GoToBase(this);
        goToWaypoint = new GoToNearestWaypoint(this);
        reload = new Reload(this);

        // Creación de las acciones a utilizar por el robot
        doPatrol = new DoPatrol();
        rotateRandom = new RotateRandomTimes();
        recalculateAim = new RecalculateAim();
        searchBase = new SearchForBase();
        searchWaypoint = new SearchforNearestWaypoint();
        reloadBullets = new ReloadBullets();
        
        // Creación de las condiciones a utilizar por los estados
        doPatrol.InitPatrol(initialWaypoint, navmesh, rotateRandom);
        rotateRandom.InitRotate(pinkRobot, doPatrol, rotationSpeed);
        recalculateAim.InitAim(robotHunt, pinkRobot, nestor, minDist, maxDist);
        searchBase.InitSearchBase(initialWaypoint, doPatrol, navmesh);
        searchWaypoint.InitNearestWaypoint(numWaypoints, initialWaypoint, navmesh, doPatrol, pinkRobot);
        reloadBullets.InitReload(pinkRobot);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
