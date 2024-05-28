using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace GameMain.Scripts.Tools.Node_Canvas_Extension
{
    [Category("Task")]
    [Description("To change a tag")]
    public class ChangeTag : ActionTask<Transform>
    {
        public enum TagType
        {
            NPC,
            Untagged
        }

        public TagType tag;
        
        protected override string info
        {
            get { return $"Change Tag To {tag}"; }
        }

        protected override void OnExecute()
        {
            base.OnExecute();

            Debug.Log(agent.name);
            agent.tag = tag.ToString();
            
            EndAction();
        }
    }
}