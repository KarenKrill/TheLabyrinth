#nullable enable

using System;
using System.Collections.Generic;

namespace KarenKrill.Common.StateSystem
{
    using Abstractions;

    public class StateMachine<T> : IStateMachine<T> where T : Enum
    {
        public T State => _switchableStateMachine.State;
        public IStateSwitcher<T> StateSwitcher => _switchableStateMachine.StateSwitcher;

        public event StateTransitionDelegate<T>? StateEnter
        {
            add => _switchableStateMachine.StateEnter += value;
            remove => _switchableStateMachine.StateEnter -= value;
        }
        public event StateTransitionDelegate<T>? StateExit
        {
            add => _switchableStateMachine.StateExit += value;
            remove => _switchableStateMachine.StateExit -= value;
        }

        public StateMachine(IDictionary<T, IList<T>> stateTransitions, T initialState)
        {
            _switchableStateMachine = new SwitchableStateMachine(stateTransitions, initialState);
        }

        private readonly SwitchableStateMachine _switchableStateMachine;

        private class SwitchableStateMachine : IStateMachine<T>, IStateSwitcher<T>
        {
            public T State => _state;
            public IStateSwitcher<T> StateSwitcher => this;

            public event StateTransitionDelegate<T>? StateEnter;
            public event StateTransitionDelegate<T>? StateExit;

            public SwitchableStateMachine(IDictionary<T, IList<T>> stateTransitions, T initialState)
            {
                _stateTransitions = stateTransitions;
                _initialState = initialState;
                _state = initialState;
            }
            public IEnumerable<T> ValidStateTransitions(T state) => _stateTransitions[state];
            public bool IsCanTransitTo(T state) => _stateTransitions[_state].Contains(state);
            public void TransitTo(T state)
            {
                if (IsCanTransitTo(state))
                {
                    try
                    {
                        StateExit?.Invoke(_state, state);
                    }
                    finally
                    {
                        var fromState = _state;
                        _state = state;
                        StateEnter?.Invoke(fromState, _state);
                    }
                }
                else
                {
                    throw new InvalidStateMachineTransitionException<T>(_state, state);
                }
            }
            public bool TryTransitTo(T state)
            {
                if (IsCanTransitTo(state))
                {
                    try
                    {
                        StateExit?.Invoke(_state, state);
                    }
                    finally
                    {
                        var fromState = _state;
                        _state = state;
                        StateEnter?.Invoke(fromState, _state);
                    }
                    return true;
                }
                return false;
            }
            public void TransitToInitial()
            {
                if (!_state.Equals(_initialState))
                {
                    StateExit?.Invoke(_state, _initialState);
                }
                var fromState = _state;
                _state = _initialState;
                StateEnter?.Invoke(fromState, _state);
            }

            private T _state;
            private readonly T _initialState;
            private readonly IDictionary<T, IList<T>> _stateTransitions;
        }
    }
}