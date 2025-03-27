using UnityEngine;
using TMPro;

namespace KarenKrill.TheLabyrinth.UI.Views
{
    using Abstractions;
    using Common.UI.Views;
    using Common.Utilities;

    public class LevelInfoView : ViewBehaviour, IILevelInfoView
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