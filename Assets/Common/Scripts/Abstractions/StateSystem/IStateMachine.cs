#nullable enable

using System;

namespace KarenKrill.StateSystem.Abstractions
{
    public delegate void StateTransitionDelegate<T>(T fromState, T toState) where T : Enum;

    public interface IStateMachine<T> where T : Enum
    {
        T State { get; }
        IStateSwitcher<T> StateSwitcher { get; }


        public event StateTransitionDelegate<T>? StateEnter;

        public event StateTransitionDelegate<T>? StateExit;
    }
}