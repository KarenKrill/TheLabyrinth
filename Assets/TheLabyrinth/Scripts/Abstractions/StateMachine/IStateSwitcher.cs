﻿using System;
using System.Collections.Generic;

namespace KarenKrill.TheLabyrinth.StateMachine.Abstractions
{
    public interface IStateSwitcher<T> where T : Enum
    {
        T State { get; }

        IEnumerable<T> ValidStateTransitions(T state);
        bool IsCanTransitTo(T state);
        /// <exception cref="InvalidStateMachineTransitionException"></exception>
        void TransitTo(T state);
        bool TryTransitTo(T state);
        void TransitToInitial();
    }
}
