using System;
using GameMain.Scripts.Controller;
using GameMain.Scripts.UI;
using GameMain.Scripts.Utility;
using NodeCanvas.DialogueTrees;
using QFramework;

namespace GameMain.Scripts.Entity.EntityLogic
{
    public class DialogueManager : MonoSingleton<DialogueManager>, ISoulstealersGameController
    {
        public void OnGameInit()
        {
            Subscribe();
            
            UIKit.OpenPanel<DialoguePanel>();
        }

        public void OnUpdate(float elapse) { }

        public void OnFixedUpdate(float elapse) { }

        public void OnGameShutdown()
        {
            UnSubscribe();
        }

        public void Subscribe()
        {
            DialogueTree.OnDialogueStarted += OnDialogueStarted;
            DialogueTree.OnDialogueFinished += OnDialogueFinished;
        }
        
        public void UnSubscribe()
        {
            DialogueTree.OnDialogueStarted -= OnDialogueStarted;
            DialogueTree.OnDialogueFinished -= OnDialogueFinished;
        }

        public void OnDialogueStarted(DialogueTree dlg)
        {
            if (InputManager.Instance.mouseInteractType.Value == InputManager.MouseInteractType.Ground)
            {
                InputManager.Instance.mouseInteractType.Value = InputManager.MouseInteractType.Dialogue;
            }
        }
        
        public void OnDialogueFinished(DialogueTree dlg)
        {
            if (InputManager.Instance.mouseInteractType.Value == InputManager.MouseInteractType.Dialogue)
            {
                InputManager.Instance.mouseInteractType.Value = InputManager.MouseInteractType.Ground;
            }
        }

        public IArchitecture GetArchitecture()
        {
            return Soulstealers.Interface;
        }
    }
}