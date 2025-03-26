using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using KarenKrill.Common.Logging;
using KarenKrill.TheLabyrinth.GameStates;
using KarenKrill.TheLabyrinth.StateMachine;
using KarenKrill.Common.UI.Views;
using KarenKrill.TheLabyrinth.GameFlow.Abstractions;
using KarenKrill.TheLabyrinth.StateMachine.Abstractions;
using KarenKrill.TheLabyrinth.GameFlow;
using KarenKrill.TheLabyrinth.Input;
using KarenKrill.TheLabyrinth.Input.Abstractions;
using KarenKrill.Common.UI.Presenters.Abstractions;
using KarenKrill.TheLabyrinth.UI;
using System.Linq;

namespace KarenKrill.TheLabyrinth
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField]
        List<GameObject> _uiPrefabs;

        private void InstallGameStateMachine()
        {
            Dictionary<GameState, IList<GameState>> validTransitions = new()
            {
                { GameState.Initial, new List<GameState> { GameState.MainMenu } },
                { GameState.MainMenu, new List<GameState> { GameState.GameStart, GameState.GameEnd } },
                { GameState.GameStart, new List<GameState> { GameState.LevelLoad } },
                { GameState.LevelLoad, new List<GameState> { GameState.LevelPlay } },
                { GameState.LevelPlay, new List<GameState> { GameState.LooseMenu, GameState.LevelFinish, GameState.PauseMenu } },
                { GameState.PauseMenu, new List<GameState> { GameState.GameEnd, GameState.GameStart, GameState.MainMenu, GameState.LevelPlay } },
                { GameState.LevelFinish, new List<GameState> { GameState.LevelLoad, GameState.WinMenu, GameState.LooseMenu } },
                { GameState.WinMenu, new List<GameState> { GameState.GameEnd, GameState.GameStart } },
                { GameState.LooseMenu, new List<GameState> { GameState.GameEnd, GameState.GameStart } },
                { GameState.GameEnd, new List<GameState>() }
            };
            Container.Bind<IStateMachine<GameState>>().To<StateMachine<GameState>>().AsSingle().WithArguments(validTransitions, GameState.Initial);
        }
        private void InstallGameStateMachine2()
        {
            Dictionary<GameState, IList<GameState>> validTransitions = new()
            {
                { GameState.Initial, new List<GameState> { GameState.MainMenu } },
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
            Container.BindInterfacesAndSelfTo<GameApp>().AsSingle();
            Container.BindInterfacesAndSelfTo<ViewFactory>().AsSingle().WithArguments(_uiPrefabs);
            Container.Bind<InitialState>().AsSingle();//.NonLazy();
            Container.Bind<MainMenuState>().AsSingle();//.NonLazy();
        }
        List<Type> GetImplementationTypes(Type interfaceType, Type[] excludeTypes)
        {
            List<Type> implementationTypes = new();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (assembly.FullName.StartsWith(nameof(KarenKrill)))
                {
                    var assemblyTypes = assembly.GetTypes();
                    foreach (var type in assemblyTypes)
                    {
                        if (interfaceType.IsAssignableFrom(type) && !excludeTypes.Contains(type))
                        {
                            implementationTypes.Add(type);
                        }
                    }
                }
            }
            return implementationTypes;
        }
        private void InstallPresenterBindings()
        {
            var presenterTypes = GetImplementationTypes(typeof(IPresenter), new Type[] { typeof(IPresenter), typeof(IPresenter<>) });
            foreach (var presenterType in presenterTypes)
            {
                Container.BindInterfacesTo(presenterType).FromNew().AsSingle();
            }
        }
        public override void InstallBindings()
        {
            Container.Bind<IInputActionService>().To<InputActionService>().FromNew().AsSingle().NonLazy();
#if DEBUG
            Container.Bind<ILogger>().To<Logger>().FromNew().AsSingle().WithArguments(new DebugLogHandler());
#else
            Container.Bind<ILogger>().To<StubLogger>().FromNew().AsSingle();
#endif
            InstallGameStateMachine();
            Container.BindInterfacesAndSelfTo<ViewFactory>().AsSingle().WithArguments(_uiPrefabs);
            Container.BindInterfacesAndSelfTo<GameplayController>().FromMethod(context =>
            {
                return GameObject.FindFirstObjectByType<GameplayController>(FindObjectsInactive.Exclude);
            }).AsTransient();
            Container.Bind<IGameFlow>().To<GameFlow.GameFlow>().FromNew().AsSingle();
            InstallPresenterBindings();
            Container.Bind<PresenterManager>().FromNew().AsSingle().NonLazy();
        }
    }
}