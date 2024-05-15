using GameMain.Scripts.Utility;
using NodeCanvas.DialogueTrees;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain.Scripts.Entity.EntityLogic
{
    public class NPC : Entity
    {
        [SerializeField]
        private DialogueTreeController controller;

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

        public void StartDialogue()
        {
            controller.StartDialogue();
        }

        public Vector3 GetDialoguePoint()
        {
            return dialoguePoint.position;
        }
    }
}