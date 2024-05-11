using Cinemachine;
using GameMain.Scripts.Utility;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityGameFramework.Runtime;

namespace GameMain.Scripts.Entity.EntityLogic
{
    public class CameraManager : Entity
    {
        [SerializeField] 
        private CinemachineFreeLook camera;

        [SerializeField] 
        private CinemachineInputProvider inputProvider;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            var Entity = GameEntry.GetComponent<EntityComponent>();
            var player = Entity.GetEntity(AssetUtility.GetEntityAsset("Player")).Logic as Player;

            camera.LookAt = player.transform;
            camera.Follow = player.transform;
        }
    }
}