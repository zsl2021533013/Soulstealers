using PixelCrushers.DialogueSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace GameMain.Scripts.Entity.EntityLogic
{
    public class DialogueInterface : Entity
    {
        [SerializeField] 
        private DialogueSystemController controller;

        [SerializeField] 
        private DialogueSystemEvents events;
    }
}