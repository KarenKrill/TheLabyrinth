using UnityEngine;

namespace KarenKrill.TheLabyrinth.UI
{
    public class CursorLocker : MonoBehaviour
    {
        private void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}