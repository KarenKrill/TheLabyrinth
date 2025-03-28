using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace KarenKrill.TheLabyrinth
{
    using Common.Logging;
    using Common.UI.Presenters.Abstractions;
    using Common.UI.Views;
    using Common.StateSystem.Abstractions;
    using GameFlow.Abstractions;
    using Input.Abstractions;
    using Common.StateSystem;
    using GameFlow;
    using Input;

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
            Container.Bind<IStateMachine<GameState>>()
                .To<StateMachine<GameState>>()
                .AsSingle()
                .WithArguments(validTransitions, GameState.Initial)
                .OnInstantiated((context, instance) =>
                {
                    if (instance is IStateMachine<GameState> stateMachine)
                    {
                        context.Container.Bind<IStateSwitcher<GameState>>().FromInstance(stateMachine.StateSwitcher);
                    }
                })
                .NonLazy();
            Container.BindInterfacesTo<GameApp>().AsSingle();
            var stateTypes = GetImplementationTypes(typeof(IStateHandler<GameState>), Type.EmptyTypes);
            foreach (var stateType in stateTypes)
            {
                Container.BindInterfacesTo(stateType).AsSingle();
            }
            Container.BindInterfacesTo<ManagedStateMachine<GameState>>().AsSingle();
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
            Container.BindInterfacesAndSelfTo<LoadLevelManager>().FromMethod(context =>
            {
                return GameObject.FindFirstObjectByType<LoadLevelManager>(FindObjectsInactive.Exclude);
            }).AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayController>().FromMethod(context =>
            {
                return GameObject.FindFirstObjectByType<GameplayController>(FindObjectsInactive.Exclude);
            }).AsSingle();
            Container.Bind<IGameFlow>().To<GameFlow.GameFlow>().FromNew().AsSingle();
            InstallPresenterBindings();
        }
    }
}