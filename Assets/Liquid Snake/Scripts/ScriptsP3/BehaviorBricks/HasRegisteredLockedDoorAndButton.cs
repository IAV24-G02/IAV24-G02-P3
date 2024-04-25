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

        [OutParam("currentWaypoint")]
        [Help("currentWaypoint")]
        public GameObject currentWaypoint;

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
                int i = 0;
                bool found = false;
                while (i < register.doors.Count && !found)
                {
                    door = register.doors[i];
                    string doorName = door.name;
                    int j = 0;
                    while (j < register.buttons.Count && !found)
                    {
                        string buttonName = register.buttons[j].name;
                        if ((doorName == "BlueSecurityGateBeams" && buttonName == "BlueButton") ||
                        (doorName == "RedSecurityGateBeams" && buttonName == "RedButton") ||
                        (doorName == "WhiteSecurityGateBeams" && buttonName == "WhiteButton") ||
                        (doorName == "GreenSecurityGateBeams" && buttonName == "GreenButton"))
                        {
                            button = register.buttons[j];
                            found = true;
                        }
                        ++j;
                    }
                    ++i;
                }
            }
            currentWaypoint = door;
            return door != null && button != null;
        }
    }
}
