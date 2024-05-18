using Cinemachine;
using GameMain.Scripts.Controller;
using GameMain.Scripts.Model;
using GameMain.Scripts.Utility;
using QFramework;
using UnityEngine;

namespace GameMain.Scripts.Entity.EntityLogic
{
    public class CameraManager : MonoSingleton<CameraManager>, ISoulstealersGameController
    {
        [SerializeField] 
        private CinemachineFreeLook cam;

        [SerializeField] 
        private CinemachineInputProvider inputProvider;
        
        public override void OnSingletonInit()
        {
        }

        public void OnGameInit()
        {
            var model = this.GetModel<PlayerModel>();
            var cameraPoint = model.cameraPoint;

            cam.LookAt = cameraPoint;
            cam.Follow = cameraPoint;
        }

        public void OnUpdate(float elapse) { }

        public void OnFixedUpdate(float elapse) { }
        
        public void OnGameShutdown() { }

        public IArchitecture GetArchitecture()
        {
            return Soulstealers.Interface;
        }
    }
}