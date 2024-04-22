using LiquidSnake.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace LiquidSnake.LevelObjects
{
    public class StarAreaTrigger : MonoBehaviour, IResetteable
    {
        [SerializeField]
        private UnityEvent onAreaEntered;

        [SerializeField] private GameObject player;

        public void Reset()
        {
            gameObject.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                onAreaEntered?.Invoke();
                gameObject.SetActive(false);
                if (player != null)
                {
                    Register reg = player.GetComponent<Register>();
                    if (reg != null)
                    {
                        reg.healthSpotsAvailable.Remove(gameObject);
                    }
                }
            }
        }
    } // StarAreaTrigger

} // namespace LiquidSnake.LevelObjects
