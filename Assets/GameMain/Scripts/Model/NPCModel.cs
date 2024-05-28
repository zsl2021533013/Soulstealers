using System.Collections.Generic;
using System.Linq;
using GameMain.Scripts.Entity.EntityLogic;
using GameMain.Scripts.Event;
using GameMain.Scripts.Scriptable_Object;
using GameMain.Scripts.Utility;
using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using QFramework;
using UnityEngine;

namespace GameMain.Scripts.Model
{
    public class NPCModel : AbstractModel
    {
        public List<NPCController> NPCs = new List<NPCController>();
        
        protected override void OnInit()
        {
            NPCs = new List<NPCController>();
            
            var data = Resources.Load<GameData>(AssetUtility.GetSaveAsset("GameData")).npcDataDic;
            
            NPCs = Object.FindObjectsOfType<NPCController>().ToList();
            NPCs.ForEach(npc =>
            {
                if (data.ContainsKey(npc.name))
                {
                    npc.Deserialize(data[npc.name]);
                }
            });
            
            DialogueTree.OnDialogueFinished += OnDialogueFinished;
        }

        protected override void OnDeinit()
        {
            base.OnDeinit();
            
            DialogueTree.OnDialogueFinished -= OnDialogueFinished;
        }

        private void OnDialogueFinished(DialogueTree obj)
        {
            this.SendEvent<ModelChangeEvent>();
        }
    }
}