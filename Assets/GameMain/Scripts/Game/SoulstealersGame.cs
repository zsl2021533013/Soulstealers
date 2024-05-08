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
    public enum MouseInteractType
    {
        Ground,
        UI
    }
    
    public class SoulstealersGame : GameBase
    {
        private Player player;
        private MouseInteractType mouseInteractType = MouseInteractType.Ground;
        
        public override void Initialize()
        {
            base.Initialize();
            
            var Event = GameEntry.GetComponent<EventComponent>();
            
            Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);

            var playerStart = Object.FindObjectOfType<PlayerStart>();

            var Entity = GameEntry.GetComponent<EntityComponent>();
            
            Entity.ShowEntity(
                typeof(DialogueInterface), 
                "Managers", 
                Constant.AssetPriority.ManagerAsset,
                new EntityData(Entity.GenerateSerialId(), Constant.EntityId.DialogueManager));
            
            Entity.ShowEntity(
                typeof(NPC), 
                "NPCs", 
                Constant.AssetPriority.NPCAsset,
                new NPCData(Entity.GenerateSerialId(), Constant.EntityId.NPC)
                {
                    Name = "NPC",
                    Position = new Vector3(0, 1, 4)
                });
            
            Entity.ShowEntity(
                typeof(Player), 
                "Player", 
                Constant.AssetPriority.PlayerAsset,
                new PlayerData(Entity.GenerateSerialId(), Constant.EntityId.Player)
                {
                    Name = "Player",
                    Position = playerStart.transform.position,
                });
            
            mouseInteractType = MouseInteractType.Ground;
            
            GameOver = false;
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);
            
            UpdateMouse();
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

            if (ne.EntityLogicType == typeof(Player))
            {
                player = ne.Entity.Logic as Player;
            }
        }

        protected void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs)e;
            Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
        }

        private void UpdateMouse()
        {
            switch (mouseInteractType )
            {
                case MouseInteractType.Ground:
                    if (Input.GetMouseButton(0))
                    {
                        var mousePosition = Input.mousePosition;
            
                        var ray = Camera.main.ScreenPointToRay(mousePosition);

                        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
                        {
                            player?.SetDestination(hit.point);
                        }
                    }
                    break;
                case MouseInteractType.UI:
                    break;
                default:
                    break;
            }
        }
    }
}