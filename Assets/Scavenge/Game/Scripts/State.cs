using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Haunt
{
    public interface IGameState
    {
        void Start();
        void Awake();
        void Update();
        void End();
        void OnTriggerAction(InputAction.CallbackContext ctx);
    }
}