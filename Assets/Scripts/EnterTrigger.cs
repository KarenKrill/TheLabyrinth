using UnityEngine;
using UnityEngine.Events;

namespace KarenKrill
{
    public class EnterTrigger : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<Collider> TriggerEnter = new();
        private void OnTriggerEnter(Collider other) => TriggerEnter.Invoke(other);
    }
}