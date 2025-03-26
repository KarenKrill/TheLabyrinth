using UnityEngine;

namespace KarenKrill.Common.UI.Views
{
    using Abstractions;
    public class ViewBehaviour : MonoBehaviour, IView
    {
        public void Close() => gameObject.SetActive(false);
        public void Show() => gameObject.SetActive(true);
    }
}