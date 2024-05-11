using GameMain.Scripts.Utility;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain.Scripts.Entity.EntityLogic
{
    public class DialogueManager : Entity
    {
        [SerializeField] 
        private DialogueSystemController controller;

        [SerializeField] 
        private DialogueSystemEvents events;

        private InputManager inputManager;

        public InputManager InputManager
        {
            get
            {
                if (inputManager == null)
                {
                    var Entity = GameEntry.GetComponent<EntityComponent>();
                    inputManager = Entity.GetEntity(AssetUtility.GetEntityAsset("Input Manager")).Logic as InputManager;
                }

                return inputManager;
            }
        }
        
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            events.conversationEvents.onConversationStart.AddListener(OnConversationStart);
            events.conversationEvents.onConversationEnd.AddListener(OnConversationEnd);
        }

        private void OnConversationStart(Transform actor)
        {
            InputManager.mouseInteractType.Value = InputManager.MouseInteractType.UI;
        }

        private void OnConversationEnd(Transform actor)
        {
            InputManager.mouseInteractType.Value = InputManager.MouseInteractType.Ground;
        }
    }
}