using System.Collections.Generic;
using GameMain.Scripts.Entity.EntityLogic;
using QFramework;

namespace GameMain.Scripts.Model
{
    public class NPCModel : AbstractModel
    {
        public List<NPCController> NPCs = new List<NPCController>();
        
        protected override void OnInit()
        {
            
        }
    }
}