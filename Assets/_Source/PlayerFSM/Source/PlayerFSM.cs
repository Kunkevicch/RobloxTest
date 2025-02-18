using System;
using System.Collections.Generic;
using UnityEngine;

namespace RobloxTest
{
    public class PlayerFSM
    {
        private readonly Dictionary<Type, PlayerStateBase> _states = new();
        private PlayerStateBase _currentState;

        public void AddState(PlayerStateBase newState)
        {
            if (_states.ContainsKey(newState.GetType()))
                return;

            _states[newState.GetType()] = newState;
        }

        public void SetState<T>() where T : PlayerStateBase
        {
            var stateType = typeof(T);

            if (!_states.ContainsKey(stateType))
                return;

            if (_currentState != null && _currentState.GetType() == stateType)
                return;

            if (_states.TryGetValue(stateType, out PlayerStateBase newState))
            {
                if (_currentState is IExitable exitable)
                {
                    exitable.Exit();
                }
                _currentState = newState;
                _currentState.Enter();
            }
        }

        public void Update()
        {
            Debug.Log(_currentState.GetType());
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
