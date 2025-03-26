using UnityEngine;
using TMPro;
using KarenKrill.Common.Utilities;
using KarenKrill.Common.UI.Views;

namespace KarenKrill.TheLabyrinth.UI.Views
{
    using Abstractions;

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