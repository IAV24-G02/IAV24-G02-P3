using Pada1.BBCore;
using BBUnity.Conditions;
using UnityEngine;

namespace BBCore.Conditions
{
    [Condition("Basic/HasRegisteredLockedDoorAndButton")]
    [Help("Checks if the target has registered a locked door and same color button")]
    public class HasRegisteredLockedDoorAndButton : GOCondition
    {
        [InParam("target")]
        [Help("target")]
        public GameObject target;

        [OutParam("door")]
        [Help("door")]
        public GameObject door;

        [OutParam("button")]
        [Help("button")]
        public GameObject button;

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

            if (register.doors.Count > 0 && register.buttons.Count > 0)
            {
                if ((register.doors[0].name == "BlueSecurityGateBeams" && register.buttons[0].name == "BlueButton") || 
                    (register.doors[0].name == "RedSecurityGateBeams" && register.buttons[0].name == "RedButton") ||
                    (register.doors[0].name == "WhiteSecurityGateBeams" && register.buttons[0].name == "WhiteButton") ||
                    (register.doors[0].name == "GreenSecurityGateBeams" && register.buttons[0].name == "GreenButton"))
                {
                    door = register.doors[0];
                    button = register.buttons[0];
                }
            }
            return door != null && button != null;
        }
    }
}
