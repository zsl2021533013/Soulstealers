using System.Collections.Generic;
using DG.Tweening;
using GameMain.Scripts.UI;
using GameMain.Scripts.Utility;
using NodeCanvas.DialogueTrees;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain.Scripts.Entity.EntityLogic
{
    public class DialogueManager : Entity
    {
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
            
            Subscribe();

            var UI = GameEntry.GetComponent<UIComponent>();
            UI.OpenUIForm(UIFormId.DialogueForm, this);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            
            UnSubscribe();
        }

        public void Subscribe()
        {
            DialogueTree.OnDialogueStarted += OnDialogueStarted;
        }
        
        public void UnSubscribe()
        {
            DialogueTree.OnDialogueStarted -= OnDialogueStarted;
        }

        public void OnDialogueStarted(DialogueTree dlg)
        {
            InputManager.mouseInteractType.Value = InputManager.MouseInteractType.Ground;
        }
    }
}