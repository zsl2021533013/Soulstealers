using GameMain.Scripts.Entity.EntityLogic;
using GameMain.Scripts.Event;
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
            pathStatus.Subscribe(status =>
            {
                if (status == NavMeshStatus.Complete)
                {
                    this.SendEvent<PlayerArriveEvent>();
                }
            });
        }
    }
}