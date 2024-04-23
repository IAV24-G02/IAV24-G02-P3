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
    [Condition("Basic/CheckHealthNotKnowing")]
    [Help("Checks if the target has low health and returns the nearest healthSpot available seen")]
    public class CheckHealthNotKnowing : GOCondition
    {
        [InParam("target")]
        [Help("target")]
        public GameObject target;

        [OutParam("nearestHealthSpotSeen")]
        [Help("nearestHealthSpotSeen")]
        public GameObject nearestHealthSpotSeen;

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
            float healthSpotDistance = Mathf.Infinity;
            if (register.healthSpots != null && register.healthSpots.Count > 0)
            {
                healthSpotDistance = Vector3.Distance(target.transform.position, register.healthSpots[0].transform.position);
                nearestHealthSpotSeen = register.healthSpots[0];
            }
            else
            {
                nearestHealthSpotSeen = null;
            }
            for (int i = 0; i < register.healthSpots.Count; ++i)
            {
                if (register.healthSpots[i] != null && register.healthSpots[i].activeSelf)
                {
                    float newDistance = Vector3.Distance(target.transform.position, register.healthSpots[i].transform.position);
                    if (newDistance < healthSpotDistance)
                    {
                        healthSpotDistance = newDistance;
                        nearestHealthSpotSeen = register.healthSpots[i];
                    }
                }
            }

            // Nota: es posible que cuando detectemos el punto de curación más cercano, este ya no esté disponible

            return target.GetComponent<Health>().CurrentValue() < minHealth && nearestHealthSpotSeen != null;
        }
    }
}
