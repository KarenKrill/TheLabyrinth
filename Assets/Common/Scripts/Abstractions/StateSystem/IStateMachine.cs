#nullable enable

using System;

namespace KarenKrill.Common.StateSystem.Abstractions
{
    public delegate void StateTransitionDelegate<T>(T state) where T : Enum;

    public interface IStateMachine<T> where T : Enum
    {
        T State { get; }
        IStateSwitcher<T> StateSwitcher { get; }


        public event StateTransitionDelegate<T>? StateEnter;

        public event StateTransitionDelegate<T>? StateExit;
    }
}