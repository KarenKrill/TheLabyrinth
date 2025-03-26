﻿using KarenKrill.Common.UI.Presenters.Abstractions;

namespace KarenKrill.TheLabyrinth.UI.Presenters
{
    using KarenKrill.TheLabyrinth.GameFlow.Abstractions;
    using Views.Abstractions;

    public class LooseMenuPresenter : IPresenter<ILooseMenuView>
    {
        public ILooseMenuView View { get; }
        readonly IGameFlow _gameFlow;
        void OnRestart()
        {
            Disable();
            _gameFlow.StartGame();
        }
        void OnExit() => _gameFlow.EndGame();
        public LooseMenuPresenter(ILooseMenuView view, IGameFlow gameFlow)
        {
            View = view;
            _gameFlow = gameFlow;
        }
        public void Enable()
        {
            View.Restart += OnRestart;
            View.Exit += OnExit;
            View.Show();
        }
        public void Disable()
        {
            View.Close();
            View.Restart -= OnRestart;
            View.Exit -= OnExit;
        }
    }
}