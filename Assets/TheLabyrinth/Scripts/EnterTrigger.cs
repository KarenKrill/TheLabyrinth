using UnityEngine;
using UnityEngine.Events;

namespace KarenKrill.TheLabyrinth
{
    public class EnterTrigger : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<Collider> _triggerEnter = new();

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"{gameObject.name} {nameof(OnTriggerEnter)} with {other.gameObject.name}");
            _triggerEnter.Invoke(other);
        }
    }
}