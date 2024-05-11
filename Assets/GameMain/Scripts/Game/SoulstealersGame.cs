using GameFramework.Event;
using GameMain.Scripts.Definition.Constant;
using GameMain.Scripts.Entity;
using GameMain.Scripts.Entity.EntityData;
using GameMain.Scripts.Entity.EntityLogic;
using GameMain.Scripts.Utility;
using Unity.VisualScripting;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain.Scripts.Game
{
    public class SoulstealersGame : GameBase
    {
        public override void Initialize()
        {
            base.Initialize();
            
            var Event = GameEntry.GetComponent<EventComponent>();
            
            Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);

            var playerStart = Object.FindObjectOfType<PlayerStart>();

            var Entity = GameEntry.GetComponent<EntityComponent>();
            
            Entity.ShowEntity(
                typeof(Player), 
                "Player", 
                Constant.AssetPriority.PlayerAsset,
                new PlayerData(Entity.GenerateSerialId(), Constant.EntityId.Player)
                {
                    Name = "Player",
                    Position = playerStart.transform.position,
                });
            
            Entity.ShowEntity(
                typeof(NPC), 
                "NPCs", 
                Constant.AssetPriority.NPCAsset,
                new NPCData(Entity.GenerateSerialId(), Constant.EntityId.NPC)
                {
                    Name = "NPC",
                    Position = new Vector3(0, 0, 4)
                });
            
            Entity.ShowEntity(
                typeof(InputManager), 
                "Managers", 
                Constant.AssetPriority.ManagerAsset,
                new EntityData(Entity.GenerateSerialId(), Constant.EntityId.InputManager));
            
            Entity.ShowEntity(
                typeof(DialogueManager), 
                "Managers", 
                Constant.AssetPriority.ManagerAsset,
                new EntityData(Entity.GenerateSerialId(), Constant.EntityId.DialogueManager));
            
            Entity.ShowEntity(
                typeof(CameraManager), 
                "Managers", 
                Constant.AssetPriority.ManagerAsset,
                new EntityData(Entity.GenerateSerialId(), Constant.EntityId.CameraManager));
            
            GameOver = false;
        }
        
        public override void Shutdown()
        {
            base.Shutdown();
            
            var Event = GameEntry.GetComponent<EventComponent>();
            
            Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
        }

        protected void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
        }

        protected void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs)e;
            Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
        }
    }
}