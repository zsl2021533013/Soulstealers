using System.Collections.Generic;
using System.Linq;
using GameMain.Scripts.Entity.EntityLogic;
using GameMain.Scripts.Scriptable_Object;
using GameMain.Scripts.Utility;
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
            
            var data = Resources.Load<GameData>(AssetUtility.GetSaveAsset("GameData")).dialogueData;
            
            NPCs = Object.FindObjectsOfType<NPCController>().ToList();
            NPCs.ForEach(npc =>
            {
                npc.Deserialize(data[npc.name]);
            });
        }
    }
}