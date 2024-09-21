using UnityEngine;
using UnityEngine.Events;

namespace Enchanter.Utils
{
    public class TriggerDelegator2D : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<Collider2D> TriggerEntered2D = new();
        [HideInInspector] public UnityEvent<Collider2D> TriggerStayed2D = new();
        [HideInInspector] public UnityEvent<Collider2D> TriggerExited2D = new();

        private void OnTriggerEnter2D(Collider2D other)
        {
            TriggerEntered2D.Invoke(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            TriggerStayed2D.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            TriggerExited2D.Invoke(other);
        }
    }
}