using UnityEngine;
using TMPro;

using KarenKrill.UI.Views;
using KarenKrill.Utilities;

namespace TheLabyrinth.UI.Views
{
    using Abstractions;

    public class LevelInfoView : ViewBehaviour, IILevelInfoView
    {
        public string RemainingTimeText { set => _remainingTimeText.text = value; }
        public string Title { set => _titleText.text = value; }
        public System.Drawing.Color RemainingTimeTextColor { set => _remainingTimeText.color = value.ToUnityColor(); }

        [SerializeField]
        private TextMeshProUGUI _titleText;
        [SerializeField]
        private TextMeshProUGUI _remainingTimeText;
    }
}