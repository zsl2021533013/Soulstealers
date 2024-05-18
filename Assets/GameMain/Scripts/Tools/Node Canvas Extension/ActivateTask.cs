using GameMain.Scripts.Entity.EntityLogic;
using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace GameMain.Scripts.Tools.Node_Canvas_Extension
{
    [Category("Task")]
    [Description("To start a task")]
    public class ActivateTask : ActionTask
    {
        public int taskId;
        
        protected override string info {
            get { return $"Start Task {taskId}"; }
        }

        protected override void OnExecute() {
            TaskManager.Instance.ActivateTask(taskId);
            TaskManager.Instance.isNewTask.Value = true;
            EndAction();
        }
    }
}