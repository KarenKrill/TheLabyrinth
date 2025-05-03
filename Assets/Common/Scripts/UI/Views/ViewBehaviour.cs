using UnityEngine;

namespace KarenKrill.UI.Views
{
    using Abstractions;

    public class ViewBehaviour : MonoBehaviour, IView
    {
        public void Close() => gameObject.SetActive(false);
        public void Show() => gameObject.SetActive(true);
    }
}