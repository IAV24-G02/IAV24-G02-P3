using Pada1.BBCore;
using BBUnity.Conditions;
using UnityEngine;

namespace BBCore.Conditions
{
    [Condition("Basic/HasRegisteredExit")]
    [Help("Checks if the target has registered the exit")]
    public class HasRegisteredExit : GOCondition
    {
        [InParam("target")]
        [Help("target")]
        public GameObject target;

        [OutParam("exit")]
        [Help("exit")]
        public GameObject exit;

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
            exit = register.exit;
            return exit != null;
        }
    }
}
