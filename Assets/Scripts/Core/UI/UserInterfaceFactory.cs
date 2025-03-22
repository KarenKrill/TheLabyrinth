using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace KarenKrill.Core.UI
{
    public class UserInterfaceFactory : IUserInterfaceFactory
    {
        [Inject]
        DiContainer _diContainer;
        List<GameObject> _viewPrefabs = new();
        private UserInterfaceFactory(List<GameObject> viewPrefabs)
        {
            _viewPrefabs = viewPrefabs;
        }

        public UIViewType Create<UIViewType>()
            where UIViewType : class
        {
            var rootUiTransform = GameObject.FindFirstObjectByType<Canvas>(FindObjectsInactive.Exclude).gameObject.transform;
            foreach (var viewPrefab in _viewPrefabs)
            {
                if (viewPrefab.TryGetComponent<UIViewType>(out _))
                {
                    var viewGameObject = _diContainer.InstantiatePrefab(viewPrefab, rootUiTransform);
                    viewGameObject.SetActive(false);
                    viewGameObject.name = viewPrefab.name;
                    return viewGameObject.GetComponent<UIViewType>();
                }
            }
            throw new InvalidOperationException($"There is no prefab for \"{typeof(UIViewType).Name}\" view");
        }
    }
}