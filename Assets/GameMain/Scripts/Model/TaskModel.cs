using System.Linq;
using GameMain.Scripts.Scriptable_Object;
using GameMain.Scripts.Utility;
using QFramework;
using UnityEngine;

namespace GameMain.Scripts.Model
{
    public class TaskModel : AbstractModel
    {
        public TaskData data;
        
        protected override void OnInit()
        {
            data = Resources.Load<TaskData>(AssetUtility.GetSOAsset("TaskData")).Instantiate();
        }

        public void ActivateTask(int id)
        {
            var task = data.tasks.FirstOrDefault(task => task.Id == id);
            if (task.state == Task.TaskState.Inactive)
            {
                task.state = Task.TaskState.Active;
            }
        }

        public void CompleteTask(int id)
        {
            var task = data.tasks.FirstOrDefault(task => task.Id == id);
            if (task.state == Task.TaskState.Active)
            {
                task.state = Task.TaskState.Complete;
            }
        }
    }
}