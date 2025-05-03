using System.Collections.Generic;
using UnityEngine;

namespace KarenKrill.UI.Views
{
    using Abstractions;

    public class ViewFactory : IViewFactory
    {
        public ViewFactory(List<GameObject> viewPrefabs)
        {
            _viewPrefabs = viewPrefabs;
        }
        public ViewType Create<ViewType>() where ViewType : IView
        {
            var rootUiTransform = Object.FindFirstObjectByType<Canvas>(FindObjectsInactive.Exclude).gameObject.transform;
            foreach (var viewPrefab in _viewPrefabs)
            {
                if (viewPrefab.TryGetComponent<ViewType>(out _))
                {
                    var viewGameObject = Object.Instantiate(viewPrefab, rootUiTransform);
                    viewGameObject.SetActive(false);
                    viewGameObject.name = viewPrefab.name;
                    return viewGameObject.GetComponent<ViewType>();
                }
            }
            throw new System.InvalidOperationException($"There is no prefab for \"{typeof(ViewType).Name}\" view");
        }

        private readonly List<GameObject> _viewPrefabs = new();
    }
}