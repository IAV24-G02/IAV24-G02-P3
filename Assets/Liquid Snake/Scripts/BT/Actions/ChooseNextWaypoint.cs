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
            // Caso 1: no tenemos seleccionado ningún waypoint, utilizamos el waypoint inicial
            if (currentWaypoint == null)
            {
                if (initialWaypoint == null)
                {
                    Debug.LogError($"initialWaypoint has not been set in enemy with name {gameObject.name}");
                    return;
                }
                nextWaypoint = initialWaypoint;
            }
            // Caso 2: tenemos un waypoint seleccionado, utilizamos su siguiente waypoint
            else
            {
                nextWaypoint = currentWaypoint.GetComponent<Waypoint>().nextWaypoint;
                if (nextWaypoint == null)
                {
                    Register register = gameObject.GetComponent<Register>();
                    if (register != null && register.rooms.Count > 0)
                    {
                        nextWaypoint = register.rooms[0];
                    }
                }
                
            }
            base.OnStart();
        }

        public override TaskStatus OnUpdate()
        {
            Debug.Log(currentWaypoint);
            base.OnUpdate();
            return currentWaypoint != null ? TaskStatus.COMPLETED : TaskStatus.FAILED;
        }
    }
}
