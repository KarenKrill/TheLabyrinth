using UnityEngine;
using TMPro;

using KarenKrill.UI.Views;

namespace TheLabyrinth.UI.Views
{
    using Abstractions;
    
    public class DiagnosticInfoView : ViewBehaviour, IDiagnosticInfoView
    {
        public string FpsText { set => _fpsText.text = value; }

        [SerializeField]
        private TextMeshProUGUI _fpsText;
    }
}