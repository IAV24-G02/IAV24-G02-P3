using Pada1.BBCore;
using BBUnity.Conditions;
using UnityEngine;

namespace BBCore.Conditions
{
    [Condition("Basic/HasRegisteredNewRooms")]
    [Help("Checks if the target has registered new rooms")]
    public class HasRegisteredNewRooms : GOCondition
    {
        [InParam("target")]
        [Help("target")]
        public GameObject target;

        [OutParam("room")]
        [Help("room")]
        public GameObject room;

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
            if (register.rooms.Count == 0)
            {
                return false;
            }
            room = register.rooms[0];
            return room != null;
        }
    }
}
