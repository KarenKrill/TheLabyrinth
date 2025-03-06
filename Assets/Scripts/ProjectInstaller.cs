using System.Collections.Generic;
using UnityEngine;
using Zenject;
using KarenKrill.Core;
using KarenKrill.Logging;

namespace KarenKrill
{
    public class ProjectInstaller : MonoInstaller
    {
        private void InstallGameStateMachine()
        {
            Dictionary<GameState, IList<GameState>> validTransitions = new()
            {
                { GameState.MainMenu, new List<GameState> { GameState.GameStart } },
                { GameState.GameStart, new List<GameState> { GameState.LevelLoad } },
                { GameState.LevelLoad, new List<GameState> { GameState.LevelPlay } },
                { GameState.LevelPlay, new List<GameState> { GameState.LooseMenu, GameState.LevelFinish, GameState.PauseMenu } },
                { GameState.PauseMenu, new List<GameState> { GameState.GameEnd, GameState.GameStart, GameState.MainMenu, GameState.LevelPlay } },
                { GameState.LevelFinish, new List<GameState> { GameState.LevelLoad, GameState.WinMenu, GameState.LooseMenu } },
                { GameState.WinMenu, new List<GameState> { GameState.GameEnd, GameState.GameStart } },
                { GameState.LooseMenu, new List<GameState> { GameState.GameEnd, GameState.GameStart } },
                { GameState.GameEnd, new List<GameState>() }
            };
            Container.Bind<IStateMachine<GameState>>().To<StateMachine<GameState>>().AsSingle().WithArguments(validTransitions, GameState.MainMenu);
        }
        public override void InstallBindings()
        {
#if DEBUG
            Container.Bind<ILogger>().To<Logger>().FromNew().AsSingle().WithArguments(new DebugLogHandler());
#else
            Container.Bind<ILogger>().To<StubLogger>().FromNew().AsSingle();
#endif
            InstallGameStateMachine();
            Container.Bind<IGameFlow>().To<GameFlow>().FromNew().AsSingle();
        }
    }
}