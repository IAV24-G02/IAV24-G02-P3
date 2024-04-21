using Pada1.BBCore;
using BBUnity.Conditions;
using UnityEngine;
using LiquidSnake.Enemies;

namespace BBCore.Conditions
{
    /// <summary>
    /// It is a basic condition to check if an enemy is looking at the target.
    /// </summary>
    [Condition("Basic/IsEnemyLooking")]
    [Help("Checks if an enemy is looking at the target")]
    public class IsEnemyLooking : GOCondition
    {
        ///<value></value>
        [InParam("target")]
        [Help("target")]
        public GameObject target;

        [OutParam("nearestHidingSpot")]
        [Help("nearestHidingSpot")]
        public GameObject nearestHidingSpot;

        /// <summary>
        /// Checks if an enemy is looking at the target.
        /// </summary>
        /// <returns>active</returns>
		public override bool Check()
        {
            if (target == null)
            {
                Debug.LogError("The target of this game object is null", gameObject);
                return false;
            }
            Register register = target.GetComponent<Register>();
            if (register == null)
            {
                Debug.LogError("The target of this game object does not have a Register component", gameObject);
                return false;
            }

            // Busca el escondite mas cercano
            // FALTA COMPROBAR SI ESTA DISPONIBLE DEPENDIENDO DE LA POSICION DEL PLAYER
            float hidingSpotDistance = Mathf.Infinity;
            if (register.hidingSpots.Count > 0 && register.hidingSpots[0] != null && register.hidingSpots[0].activeSelf)
            {
                nearestHidingSpot = register.hidingSpots[0];
            }
            for (int i = 0; i < register.hidingSpots.Count; ++i)
            {
                if (register.hidingSpots[i] != null && register.hidingSpots[i].activeSelf)
                {
                    float newDistance = Vector3.Distance(target.transform.position, register.hidingSpots[i].transform.position);
                    if (newDistance < hidingSpotDistance)
                    {
                        hidingSpotDistance = newDistance;
                        nearestHidingSpot = register.hidingSpots[i];
                    }
                }
            }

            // Comprueba si algún enemigo ha detectado al jugador
            int j = 0;
            bool detected = false;
            while (j < register.enemies.Count && !detected)
            {
                if (register.enemies[j] != null)
                {
                    BehaviorExecutor enemyExecutor = register.enemies[j].GetComponent<BehaviorExecutor>();
                    if (enemyExecutor != null)
                    {
                        if (enemyExecutor.blackboard.objectParamsNames.Contains("visionSensorObject"))
                        {
                            int index = enemyExecutor.blackboard.objectParamsNames.IndexOf("visionSensorObject");
                            GameObject visionSensor = enemyExecutor.blackboard.objectParams[index] as GameObject;
                            if (visionSensor != null)
                            {
                                VisionSensor vision = visionSensor.GetComponent<VisionSensor>();
                                if (vision != null && vision.GetClosestTarget() != null)
                                {
                                    detected = true;
                                }
                            }
                        }
                    }
                }
                ++j;
            }
            return nearestHidingSpot != null && detected;
        }
    }
}