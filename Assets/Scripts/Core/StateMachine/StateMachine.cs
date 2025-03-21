using System;
using System.Collections.Generic;

namespace KarenKrill.Core.StateMachine
{
    internal class StateMachine<T> : IStateMachine<T> where T : Enum
    {
        private IDictionary<T, IList<T>> _stateTransitions;
        private T _state;
        public StateMachine(IDictionary<T, IList<T>> stateTransitions, T initialState)
        {
            _stateTransitions = stateTransitions;
            _state = initialState;
        }

        #region IStateMachine<T>

        public T State => _state;
        public event StateTransitionDelegate<T> StateEnter;
        public event StateTransitionDelegate<T> StateExit;
        public IEnumerable<T> ValidStateTransitions(T state) => _stateTransitions[_state];
        public bool IsCanTransitTo(T state) => _stateTransitions[_state].Contains(state);
        public void TransitTo(T state)
        {
            if (IsCanTransitTo(state))
            {
                try
                {
                    StateExit?.Invoke(this, _state);
                }
                finally
                {
                    _state = state;
                    StateEnter?.Invoke(this, _state);
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
                    StateExit?.Invoke(this, _state);
                }
                finally
                {
                    _state = state;
                    StateEnter?.Invoke(this, _state);
                }
                return true;
            }
            return false;
        }

        #endregion
    }
}