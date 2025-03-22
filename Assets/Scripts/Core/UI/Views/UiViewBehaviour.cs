using UnityEngine;

namespace KarenKrill.Core.UI.Views
{
    public class UiViewBehaviour : MonoBehaviour, IUserInterfaceView
    {
        public void Close() => gameObject.SetActive(false);
        public void Show() => gameObject.SetActive(true);
    }
}