using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;
using LiquidSnake.Enemies;

namespace LiquidSnake.BT.Actions
{
    [Action("LiquidSnake/ChooseNextWaypoint")]
    [Help("From a list of waypoints (GameObjects), chooses the next one and stores it")]
    public class ChooseNextWaypoint : GOAction
    {
        [InParam("initialWaypoint")]
        [Help("Default waypoint to use on an initial run.")]
        public GameObject initialWaypoint;

        [InParam("currentWaypoint")]
        [Help("Current waypoint")]
        public GameObject currentWaypoint = null;

        [OutParam("NextWaypoint")]
        [Help("Next waypoint")]
        public GameObject nextWaypoint = null;

        public override void OnStart()
        {
            Register register = gameObject.GetComponent<Register>();
            // Caso 1: no tenemos seleccionado ningún waypoint, utilizamos el waypoint inicial
            if (currentWaypoint == null)
            {
                bool found = false;
                if (register.waypoints.Count > 0)
                {
                    int i = 0;
                    while (i < register.waypoints.Count && !found)
                    {
                        if (register.waypoints[i].activeSelf && register.waypoints[i] != initialWaypoint)
                        {
                            currentWaypoint = register.waypoints[i];
                            nextWaypoint = register.waypoints[i];
                            found = true;
                        }
                        ++i;
                    }
                }
                if (!found)
                {
                    if (register.rooms.Count > 0)
                    {
                        if (register.rooms[0].tag == "SF_Door")
                        {
                            MeshRenderer meshRenderer = register.rooms[0].GetComponent<MeshRenderer>();
                            if (meshRenderer == null || !meshRenderer.enabled)
                            {
                                nextWaypoint = register.rooms[0];
                            }
                        }
                        else
                        {
                            nextWaypoint = register.rooms[0];
                        }
                    }
                    else if (initialWaypoint == null)
                    {
                        Debug.LogError($"initialWaypoint has not been set in enemy with name {gameObject.name}");
                        return;
                    }
                    else
                    {
                        nextWaypoint = initialWaypoint;
                    }
                }
            }
            // Caso 2: tenemos un waypoint seleccionado, utilizamos su siguiente waypoint
            else
            {
                nextWaypoint = currentWaypoint.GetComponent<Waypoint>().nextWaypoint;
            }
            base.OnStart();
        }

        public override TaskStatus OnUpdate()
        {
            base.OnUpdate();
            return currentWaypoint != null ? TaskStatus.COMPLETED : TaskStatus.FAILED;
        }
    }
}
