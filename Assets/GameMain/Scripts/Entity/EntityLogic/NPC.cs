using GameMain.Scripts.Utility;
using PixelCrushers.DialogueSystem.Wrappers;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain.Scripts.Entity.EntityLogic
{
    public class NPC : Entity
    {
        [SerializeField]
        private DialogueSystemTrigger trigger;

        [SerializeField] 
        private Transform dialoguePoint;
        
        private InputManager inputManager;

        public InputManager InputManager
        {
            get
            {
                if (inputManager == null)
                {
                    var Entity = GameEntry.GetComponent<EntityComponent>();
                    inputManager = Entity.GetEntity(AssetUtility.GetEntityAsset("InputManager")).Logic as InputManager;
                }

                return inputManager;
            }
        }

        public void StartDialogue(Transform actor)
        {
            trigger.OnUse(actor);
        }

        public Vector3 GetDialoguePoint()
        {
            return dialoguePoint.position;
        }
    }
}