using System.Collections.Generic;
using UnityEngine;

namespace KarenKrill.Common.UI.Views
{
    using Abstractions;
    public class UserInterfaceFactory : IUserInterfaceFactory
    {
        List<GameObject> _viewPrefabs = new();
        public UserInterfaceFactory(List<GameObject> viewPrefabs)
        {
            _viewPrefabs = viewPrefabs;
        }
        public UIViewType Create<UIViewType>() where UIViewType : IUserInterfaceView
        {
            var rootUiTransform = Object.FindFirstObjectByType<Canvas>(FindObjectsInactive.Exclude).gameObject.transform;
            foreach (var viewPrefab in _viewPrefabs)
            {
                if (viewPrefab.TryGetComponent<UIViewType>(out _))
                {
                    var viewGameObject = Object.Instantiate(viewPrefab, rootUiTransform);
                    viewGameObject.SetActive(false);
                    viewGameObject.name = viewPrefab.name;
                    return viewGameObject.GetComponent<UIViewType>();
                }
            }
            throw new System.InvalidOperationException($"There is no prefab for \"{typeof(UIViewType).Name}\" view");
        }
    }
}