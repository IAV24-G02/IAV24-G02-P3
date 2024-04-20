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
        ///<value></value>
        [InParam("healthSpot")]
        [Help("healthSpot")]
        public GameObject healthSpot;

        ///<value></value>
        [InParam("target")]
        [Help("target")]
        public GameObject target;

        /// <summary>
        /// Checks if the healthSpot is active or not and if the target has low health.
        /// </summary>
        /// <returns>active</returns>
		public override bool Check()
        {
            return healthSpot != null && target.GetComponent<Health>().CurrentValue() < 250;
        }
    }
}
