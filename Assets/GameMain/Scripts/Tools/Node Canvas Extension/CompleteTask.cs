using GameMain.Scripts.Entity.EntityLogic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace GameMain.Scripts.Tools.Node_Canvas_Extension
{
    [Category("Task")]
    [Description("To complete a task")]
    public class CompleteTask : ActionTask
    {
        public int taskId;
        
        protected override string info {
            get { return $"Complete Task {taskId}"; }
        }

        protected override void OnExecute() {
            TaskManager.Instance.CompleteTask(taskId);
            TaskManager.Instance.isNewTask.Value = true;
            EndAction();
        }
    }
}