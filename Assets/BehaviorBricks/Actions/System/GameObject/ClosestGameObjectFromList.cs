/*using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using System.Collections.Generic;

namespace BBUnity.Actions
{
    [Action("GameObject/ClosestGameObjectFromList")]
    public class ClosestGameObjectFromList : GOAction
    {
        [InParam("list")]
        public List<GameObject> list;
        [OutParam("foundGameObject")]
        public GameObject foundGameObject;

        public override void OnStart()
        {
            float dist = float.MaxValue;
            foreach (GameObject go in list)
            {
                float newdist = (go.transform.position + foundGameObject.transform.position).sqrMagnitude;
                if (newdist < dist)
                {
                    dist = newdist;
                    foundGameObject = go;
                }
            }
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}
*/