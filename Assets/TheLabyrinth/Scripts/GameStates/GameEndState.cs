using UnityEngine;

namespace KarenKrill.TheLabyrinth.GameStates
{
    using Common.StateSystem.Abstractions;
    using GameFlow.Abstractions;

    public class GameEndState : IStateHandler<GameState>
    {
        public GameState State => GameState.GameEnd;

        public GameEndState(ILogger logger)
        {
            _logger = logger;
        }
        public void Enter()
        {
            _logger.Log($"{GetType().Name}.{nameof(Enter)}()");
            // TODO: get user confirmation on exit
            OnExitConfirmed();
        }
        public void Exit()
        {
            _logger.Log($"{GetType().Name}.{nameof(Exit)}()");
        }

        private readonly ILogger _logger;

        private void OnExitConfirmed()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
