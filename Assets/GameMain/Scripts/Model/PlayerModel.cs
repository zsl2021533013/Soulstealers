using GameMain.Scripts.Entity.EntityLogic;
using GameMain.Scripts.Event;
using GameMain.Scripts.Scriptable_Object;
using GameMain.Scripts.Utility;
using QFramework;
using UniRx;
using UnityEngine;

namespace GameMain.Scripts.Model
{
    public class PlayerModel : AbstractModel
    {
        public enum NavMeshStatus
        {
            Running,
            Complete
        }

        public Transform transform;
        public Transform cameraPoint;
        public PlayerController controller;
        
        public ReactiveProperty<NavMeshStatus> pathStatus = new ReactiveProperty<NavMeshStatus>()
        {
            Value = NavMeshStatus.Complete
        };
        
        protected override void OnInit()
        {
            LoadPlayer();
            
            pathStatus.Subscribe(status =>
            {
                if (status == NavMeshStatus.Complete)
                {
                    this.SendEvent<PlayerArriveEvent>();
                }
            });
        }

        private void LoadPlayer()
        {
            var data = Resources.Load<GameData>(AssetUtility.GetSaveAsset("GameData")).playerData;
            var player = Resources.Load<GameObject>(AssetUtility.GetCharacterAsset("Player"));
            var c = player.Instantiate(data.position, data.rotation);
            
            transform = c.transform;
            cameraPoint = c.transform.Find("Camera Point");
            controller = c.GetComponent<PlayerController>();
        }
    }
}