using Pada1.BBCore;
using BBUnity.Conditions;
using UnityEngine;
using LiquidSnake.Character;
using LiquidSnake.Enemies;
using System.Linq;
using System.Collections.Generic;

namespace BBCore.Conditions
{
    /// <summary>
    /// It is a basic condition to check if the target has low health.
    /// </summary>
    [Condition("Basic/CheckHealth")]
    [Help("Checks if the target has low health")]
    public class CheckHealth : GOCondition
    {
        [InParam("visionSensorObject")]
        [Help("Vision Sensor used by this condition with a VisionSensor Component attached")]
        public GameObject visionSensorObject;

        [InParam("target")]
        [Help("target")]
        public GameObject target;

        [OutParam("nearestHealthSpot")]
        [Help("nearestHealthSpot")]
        public GameObject nearestHealthSpot;

        [InParam("minHealth")]
        [Help("minHealth")]
        public int minHealth;

        [InParam("maxDistance")]
        [Help("maxDistance")]
        public float maxDistance = 100.0f;

        /// <summary>
        /// Checks if the nearestHealthSpot is active or not and if the target has low health.
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

            // Busca el punto de curacion mas cercano
            //float healthSpotDistance = Mathf.Infinity;
            //if (register.healthSpots.Count > 0 && register.healthSpots[0] != null && register.healthSpots[0].activeSelf)
            //{
            //    nearestHealthSpot = register.healthSpots[0];
            //}
            //for (int i = 0; i < register.healthSpots.Count; ++i)
            //{
            //    if (register.healthSpots[i] != null && register.healthSpots[i].activeSelf)
            //    {
            //        float newDistance = Vector3.Distance(target.transform.position, register.healthSpots[i].transform.position);
            //        if (newDistance < healthSpotDistance)
            //        {
            //            healthSpotDistance = newDistance;
            //            nearestHealthSpot = register.healthSpots[i];
            //        }
            //    }
            //}
            
            //int i = 0; bool found = false;
            //while (i < register.healthSpotsAvailable.Count && !found)
            //{
            //    // Buscar el más cercano
            //    KeyValuePair<GameObject, List<GameObject>> entry = register.healthSpotsAvailable.ElementAt(i);
            //    float newDistance = Vector3.Distance(target.transform.position, entry.Key.transform.position);
            //    if (newDistance < healthSpotDistance)
            //    {
            //        healthSpotDistance = newDistance;
            //        nearestHealthSpot = entry.Key;

            //        // Si tiene al menos una puerta asignada de desbloqueo
            //        if (entry.Value.Count > 0)
            //        {
            //            // Comprobar si la/s puerta/s asignada/s de desbloqueo está/n desactivada/s
            //            for (int j = 0; j < entry.Value.Count; ++j)
            //            {
            //                // Si se detecta la puerta con el sensor de visión y está desactivada, se puede curar
            //                if (visionSensorObject != null && visionSensorObject.TryGetComponent<VisionSensor>(out var visionSensor))
            //                {
            //                    GameObject closestTarget = visionSensor.GetClosestTarget();
            //                    if (closestTarget == null || (closestTarget != null && closestTarget == entry.Value[j] && !closestTarget.activeSelf))
            //                    {
            //                        healthAvailable = true;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    i++;
            //}

            float healthSpotDistance = Mathf.Infinity;
            // Cogemos el primer punto de curación del diccionario
            if (register.healthSpotsAvailable != null && register.healthSpotsAvailable.Count > 0)
            {
                KeyValuePair<GameObject, List<GameObject>> firstEntry = register.healthSpotsAvailable.First();
                GameObject firstKey = firstEntry.Key;
                nearestHealthSpot = firstKey;
                healthSpotDistance = Vector3.Distance(target.transform.position, firstKey.transform.position);
            }
            int i = 0;
            bool healthAvailable = false;
            while (register.healthSpotsAvailable != null && i < register.healthSpotsAvailable.Count && !healthAvailable && healthSpotDistance < maxDistance)
            {
                // Buscar el más cercano
                KeyValuePair<GameObject, List<GameObject>> entry = register.healthSpotsAvailable.ElementAt(i);
                float newDistance = Vector3.Distance(target.transform.position, entry.Key.transform.position);

                if (newDistance < healthSpotDistance)
                {
                    healthSpotDistance = newDistance;
                    nearestHealthSpot = entry.Key;

                    if (healthSpotDistance < maxDistance)
                    {
                        // Si tiene al menos una puerta asignada de desbloqueo
                        if (entry.Value != null && entry.Value.Count > 0)
                        {
                            // Comprobar si la/s puerta/s asignada/s de desbloqueo está/n desactivada/s
                            for (int j = 0; j < entry.Value.Count; ++j)
                            {
                                if (visionSensorObject != null && visionSensorObject.TryGetComponent<VisionSensor>(out var visionSensor))
                                {
                                    GameObject closestTarget = visionSensor.GetClosestTarget();
                                    if (closestTarget == null && (closestTarget != null && closestTarget == entry.Value[j] && !closestTarget.activeSelf))
                                    {
                                        healthAvailable = true;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            healthAvailable = true;
                        }
                    }
                }
                i++;
            }
            if (!healthAvailable || healthSpotDistance >= maxDistance)
            {
                nearestHealthSpot = null;
            }

            return target.GetComponent<Health>().CurrentValue() < minHealth && nearestHealthSpot != null
                && healthAvailable;
        }
    }
}
