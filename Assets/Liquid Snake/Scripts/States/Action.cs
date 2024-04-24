using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
    public GameObject enemy;

    // Start is called before the first frame update
    public virtual void Start() { }

    // Update is called once per frame
    public virtual void Update() { }
}

public class DoPatrol : Action
{
    private List<Transform> waypoints;

    private Transform enemyTransform;
    private Transform nextWaypoint;

    private float enemySpeed = 5.0f;

    public override void Start()
    {
        enemyTransform = enemy.GetComponent<Transform>();
    }

    public override void Update()
    {
        Vector3 dir = nextWaypoint.position - enemyTransform.position;

        if (dir.magnitude > 1.0f)
        {
            dir.Normalize();

            enemyTransform.position += dir * enemySpeed * Time.deltaTime;
        }
    }
}

public class RobotHunt : Action
{
    private GameObject player;

    private Transform playerTransform;
    private Transform enemyTransform;

    private float enemySpeed = 5.0f;

    public override void Start()
    {
        playerTransform = player.GetComponent<Transform>();
        enemyTransform = enemy.GetComponent<Transform>();
    }

    public override void Update()
    {
        Vector3 dir = playerTransform.position - enemyTransform.position;

        if (dir.magnitude > 1.0f)
        {
            dir.Normalize();

            enemyTransform.position += dir * enemySpeed * Time.deltaTime;
        }
    }
}