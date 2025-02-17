using System;
using System.Collections.Generic;

namespace RobloxTest
{
    public abstract class PlayerStateSuperBase : PlayerStateBase, IRootState, IExitable, IFixedUpdateable
    {
        private Dictionary<Type, PlayerStateBase> _subStates = new();
        private PlayerStateBase _currentSubState;

        private IUpdateable _updateableState;
        private IFixedUpdateable _fixedUpdateableState;

        protected PlayerStateSuperBase(PlayerFSM playerFSM, PlayerController controller) : base(playerFSM, controller)
        {
        }

        public void AddState(PlayerStateBase newState)
        {
            if (_subStates.ContainsKey(newState.GetType()))
                return;

            _subStates[newState.GetType()] = newState;
        }

        public void SetSubState<T>() where T : PlayerStateBase
        {
            var stateType = typeof(T);

            if (!_subStates.ContainsKey(stateType))
                return;

            if (_currentSubState != null && _currentSubState.GetType() == stateType)
                return;

            if (_subStates.TryGetValue(stateType, out PlayerStateBase newState))
            {
                ExitState(_currentSubState);

                SetSubstateUpdateType(ref _updateableState, newState);
                SetSubstateUpdateType(ref _fixedUpdateableState, newState);
                //_updateableState = newState is IUpdateable updateable ? updateable : null;
                //_fixedUpdateableState = newState is IFixedUpdateable fixedUpdateable ? fixedUpdateable : null;

                _currentSubState = newState;
                _currentSubState.Enter();
            }
        }

        public virtual void Update()
        {
            UpdateSubState();
        }

        public void UpdateSubState()
        {
            if (_updateableState != null)
            {
                _updateableState.Update();
            }
        }

        public void FixedUpdate()
        {
            if (_fixedUpdateableState != null)
            {
                _fixedUpdateableState.FixedUpdate();
            }
        }

        public void Exit()
        {
            if (_currentSubState is IExitable exitable)
            {
                exitable.Exit();
            }
        }

        private void ExitState(PlayerStateBase exitState)
        {
            if (exitState is IExitable exitable)
            {
                exitable.Exit();
            }
        }

        private void SetSubstateUpdateType<T>(ref T updateAbleState, PlayerStateBase playerState) where T : class
        => updateAbleState = playerState as T;

    }
}
