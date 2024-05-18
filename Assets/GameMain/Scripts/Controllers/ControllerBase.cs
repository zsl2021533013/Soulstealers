using QFramework;
using UnityEngine;

namespace GameMain.Scripts.Controller
{
    public class ControllerBase : MonoBehaviour, ISoulstealersGameController
    {
        public virtual void OnGameInit() { }

        public virtual void OnUpdate(float elapse) { }

        public virtual void OnFixedUpdate(float elapse) { }
        
        public virtual void OnGameShutdown() { }

        public IArchitecture GetArchitecture()
        {
            return Soulstealers.Interface;
        }
    }
}