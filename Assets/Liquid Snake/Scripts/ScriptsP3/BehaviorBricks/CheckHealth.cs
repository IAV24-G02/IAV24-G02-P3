using Pada1.BBCore;
using BBUnity.Conditions;
using UnityEngine;
using LiquidSnake.Character;
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

            // Cogemos el primer punto de curación del diccionario
            KeyValuePair<GameObject, List<GameObject>> entry = new KeyValuePair<GameObject, List<GameObject>>();
            bool healthAvailable = false;
            if (register.healthSpotsAvailable != null && register.healthSpotsAvailable.Count > 0)
            {
                entry = register.healthSpotsAvailable.First();
                GameObject firstKey = entry.Key;
                if (firstKey != null && firstKey.activeSelf)
                {
                    nearestHealthSpot = firstKey;
                }
            }

            // Si el más cercano tiene al menos una puerta asignada de desbloqueo
            if (entry.Value != null && entry.Value.Count > 0)
            {
                // Comprobar si la/s puerta/s asignada/s de desbloqueo está/n desactivada/s
                int j = 0; bool found = false;
                while (j < entry.Value.Count && !found)
                {
                    MeshRenderer doorMeshRenderer = entry.Value[j].GetComponent<MeshRenderer>();
                    if (entry.Value[j] != null && doorMeshRenderer != null && doorMeshRenderer.enabled)
                    {
                        found = true;
                    }
                    ++j;
                }
                if (!found)
                {
                    healthAvailable = true;
                }
            }
            else
            {
                healthAvailable = true;
            }

            return target.GetComponent<Health>().CurrentValue() < minHealth && nearestHealthSpot != null
                && healthAvailable;
        }
    }
}
