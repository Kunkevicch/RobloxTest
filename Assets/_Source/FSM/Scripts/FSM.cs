using System;
using System.Collections.Generic;
using UnityEngine;

namespace RobloxTest
{
    public class FSM
    {
        private Dictionary<Type, StateBase> _states = new();
        private StateBase _currentState;

        public void AddState(StateBase newState)
        {
            if (_states.ContainsKey(newState.GetType()))
                return;

            _states.Add(newState.GetType(), newState);
        }

        public void SetState<T>() where T : StateBase
        {
            var stateType = typeof(T);

            if (!_states.ContainsKey(stateType))
                return;

            if (_currentState != null && _currentState.GetType() == stateType)
                return;

            if (_states.TryGetValue(stateType, out StateBase newState))
            {
                if (_currentState is IExitable state)
                {
                    state.Exit();
                }
                _currentState = newState;
                _currentState.Enter();
            }
        }

        public void Update()
        {
            if (_currentState is IUpdateable updateable)
            {
                updateable.Update();
            }
        }

        public void FixedUpdate()
        {
            if (_currentState is IFixedUpdateable fixedUpdateable)
            {
                fixedUpdateable.FixedUpdate();
            }
        }
    }
}
