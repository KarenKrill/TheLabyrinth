using UnityEngine;
using TMPro;

namespace KarenKrill.UI.Views
{
    using Core.UI.Views;
    using KarenKrill.Core.Utilities;

    public class LevelInfoView : UiViewBehaviour, IILevelInfoView
    {
        [SerializeField]
        TextMeshProUGUI _titleText;
        [SerializeField]
        TextMeshProUGUI _remainingTimeText;
        public string RemainingTimeText { set => _remainingTimeText.text = value; }
        public string Title { set => _titleText.text = value; }
        public System.Drawing.Color RemainingTimeTextColor { set => _remainingTimeText.color = value.ToUnityColor(); }
    }
}