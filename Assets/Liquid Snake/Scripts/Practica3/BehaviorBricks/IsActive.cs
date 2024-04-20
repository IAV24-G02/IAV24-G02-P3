using Pada1.BBCore;
using BBUnity.Conditions;
using UnityEngine;

namespace BBCore.Conditions
{
    /// <summary>
    /// It is a basic condition to check if the target is active or not.
    /// </summary>
    [Condition("Basic/IsActive")]
    [Help("Checks if the target is active or not")]
    public class IsActive : GOCondition
    {
        ///<value></value>
        [InParam("target")]
        [Help("target")]
        public GameObject target;

        /// <summary>
        /// Checks if the target is active or not.
        /// </summary>
        /// <returns>active</returns>
		public override bool Check()
        {
            return target.activeSelf;
        }
    }
}