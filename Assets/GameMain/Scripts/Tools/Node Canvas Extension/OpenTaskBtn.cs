using GameMain.Scripts.UI;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using QFramework;

namespace GameMain.Scripts.Tools.Node_Canvas_Extension
{
    [Category("Task")]
    [Description("Open task button")]
    public class OpenTaskBtn : ActionTask
    {
        protected override string info
        {
            get { return $"Open task button"; }
        }

        protected override void OnExecute()
        {
            var panel = UIKit.GetPanel<TaskPanel>();
            panel.openBtn.gameObject.SetActive(true);

            EndAction();
        }
    }
}