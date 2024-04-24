using LiquidSnake.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace LiquidSnake.LevelObjects
{
    public class WaypointTrigger : MonoBehaviour, IResetteable
    {
        [SerializeField]
        private UnityEvent onAreaEntered;

        public void Reset()
        {
            gameObject.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                onAreaEntered?.Invoke();
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                onAreaEntered?.Invoke();
            }
        }
    } // WaypointTrigger

} // namespace LiquidSnake.LevelObjects
