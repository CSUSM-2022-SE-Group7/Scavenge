using System;
using UnityEngine;

namespace LocalV2
{
    public class StateManager : MonoBehaviour
    {
        private static IState _currentState;
        public void ChangeState(IState newState)
        {
            _currentState?.ExitState();

            _currentState = newState;
            
            _currentState.EnterState();
        }
 
        public void Update()
        {
            _currentState?.Execute();
        }

        public static string CurrentState()
        {
            return _currentState.Name;
        }

    }
}