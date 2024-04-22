using Pada1.BBCore;
using BBUnity.Conditions;
using UnityEngine;
using LiquidSnake.Character;

namespace BBCore.Conditions
{
    /// <summary>
    /// It is a basic condition to check if the target has low health.
    /// </summary>
    [Condition("Basic/CheckHealth")]
    [Help("Checks if the target has low health")]
    public class CheckHealth : GOCondition
    {
        [InParam("target")]
        [Help("target")]
        public GameObject target;

        [OutParam("nearestHealthSpot")]
        [Help("nearestHealthSpot")]
        public GameObject nearestHealthSpot;

        [InParam("minHealth")]
        [Help("minHealth")]
        public int minHealth = 250;

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
            // FALTA COMPROBAR SI ESTA DISPONIBLE
            float healthSpotDistance = Mathf.Infinity;
            if (register.healthSpots.Count > 0 && register.healthSpots[0] != null && register.healthSpots[0].activeSelf)
            {
                nearestHealthSpot = register.hidingSpots[0];
            }
            for (int i = 0; i < register.healthSpots.Count; ++i)
            {
                if (register.healthSpots[i] != null && register.healthSpots[i].activeSelf)
                {
                    float newDistance = Vector3.Distance(target.transform.position, register.healthSpots[i].transform.position);
                    if (newDistance < healthSpotDistance)
                    {
                        healthSpotDistance = newDistance;
                        nearestHealthSpot = register.healthSpots[i];
                    }
                }
            }

            return nearestHealthSpot != null && target.GetComponent<Health>().CurrentValue() < minHealth;
        }
    }
}
