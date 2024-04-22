using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{
    [Action("Animation/AnimateTarget")]
    [Help("Animates")]
    public class AnimateTarget : GOAction
    {
        private Animator _animator;

        [InParam("walkSpeed")]
        [Help("The speed value at which the walk animation plays.")]
        public float walkSpeed;

        public override void OnStart()
        {
            _animator = gameObject.GetComponent<Animator>();
            if (_animator == null)
            {
                Debug.LogError("No animator found in game object with name " + gameObject.name);
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (_animator == null)
                return TaskStatus.FAILED;
            _animator.enabled = true;
            _animator.SetFloat("Speed", walkSpeed);
            return TaskStatus.COMPLETED;
        }
    }
}
