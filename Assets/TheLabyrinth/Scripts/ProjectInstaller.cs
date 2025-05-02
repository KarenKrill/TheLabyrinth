using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

using KarenKrill.StateSystem.Abstractions;
using KarenKrill.UI.Presenters.Abstractions;
using KarenKrill.StateSystem;
using KarenKrill.Logging;
using KarenKrill.UI.Views;
using KarenKrill.Diagnostics;
using KarenKrill.Utilities;

namespace TheLabyrinth
{
    using Abstractions;
    using GameFlow.Abstractions;
    using Input.Abstractions;
    using GameFlow;
    using Input;
    using Movement;

    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallSettings();
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
            Container.BindInterfacesAndSelfTo<LevelController>().FromMethod(context =>
            {
                return GameObject.FindFirstObjectByType<LevelController>(FindObjectsInactive.Exclude);
            }).AsSingle();
            Container.BindInterfacesAndSelfTo<AiMoveController>().FromMethod(context =>
            {
                return GameObject.FindFirstObjectByType<AiMoveController>(FindObjectsInactive.Exclude);
            }).AsSingle();
            Container.BindInterfacesAndSelfTo<CharacterMoveController>().FromMethod(context =>
            {
                return GameObject.FindFirstObjectByType<CharacterMoveController>(FindObjectsInactive.Exclude);
            }).AsSingle();
            Container.BindInterfacesAndSelfTo<PhysicMoveBehaviour>().FromMethod(context =>
            {
                return GameObject.FindFirstObjectByType<PhysicMoveBehaviour>(FindObjectsInactive.Exclude);
            }).AsSingle();
            Container.BindInterfacesAndSelfTo<ManualMoveController>().FromMethod(context =>
            {
                return GameObject.FindFirstObjectByType<ManualMoveController>(FindObjectsInactive.Exclude);
            }).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerMoveController>().FromMethod(context =>
            {
                return GameObject.FindFirstObjectByType<PlayerMoveController>(FindObjectsInactive.Exclude);
            }).AsSingle();
            Container.Bind<IGameFlow>().To<GameFlow.GameFlow>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<DiagnosticsProvider>().FromMethod(context =>
            {
                return GameObject.FindFirstObjectByType<DiagnosticsProvider>(FindObjectsInactive.Exclude);
            }).AsSingle();
            InstallPresenterBindings();
        }

        [SerializeField]
        List<GameObject> _uiPrefabs;

        private void InstallSettings()
        {
            var qualityLevel = PlayerPrefs.GetInt("Settings.Graphics.QualityLevel", (int)QualityLevel.High);
            if (qualityLevel < 0 || qualityLevel > (int)QualityLevel.High)
            {
                qualityLevel = (int)QualityLevel.High;
            }
            var showFps = PlayerPrefs.GetInt("Settings.Diagnostic.ShowFps", 0);
            GameSettings gameSettings = new((QualityLevel)qualityLevel, showFps != 0);
            Container.Bind<GameSettings>().To<GameSettings>().FromInstance(gameSettings);
        }
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
                { GameState.WinMenu, new List<GameState> { GameState.MainMenu, GameState.GameEnd, GameState.GameStart } },
                { GameState.LooseMenu, new List<GameState> { GameState.MainMenu, GameState.GameEnd, GameState.GameStart } },
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
            var stateTypes = ReflectionUtilities.GetInheritorTypes(typeof(IStateHandler<GameState>), Type.EmptyTypes);
            foreach (var stateType in stateTypes)
            {
                Container.BindInterfacesTo(stateType).AsSingle();
            }
            Container.BindInterfacesTo<ManagedStateMachine<GameState>>().AsSingle();
        }
        private void InstallPresenterBindings()
        {
            var presenterTypes = ReflectionUtilities.GetInheritorTypes(typeof(IPresenter), new Type[] { typeof(IPresenter), typeof(IPresenter<>) });
            foreach (var presenterType in presenterTypes)
            {
                Container.BindInterfacesTo(presenterType).FromNew().AsSingle();
            }
        }
    }
}