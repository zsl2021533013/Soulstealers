using System.Collections.Generic;
using System.Linq;
using GameMain.Scripts.Event;
using GameMain.Scripts.Scriptable_Object;
using GameMain.Scripts.Utility;
using QFramework;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameMain.Scripts.Model
{
    public class TaskModel : AbstractModel
    {
        public List<Task> tasks;
        
        protected override void OnInit()
        {
            var data = Resources.Load<GameData>(AssetUtility.GetSaveAsset("GameData")).tasks;

            tasks = data;

            Soulstealers.Interface.RegisterEvent<ModelChangeEvent>(e =>
            {
                
            });
        }

        public void ActivateTask(int id)
        {
            var task = tasks.FirstOrDefault(task => task.Id == id);
            if (task.state == Task.TaskState.Inactive)
            {
                task.state = Task.TaskState.Active;
            }
        }

        public void CompleteTask(int id)
        {
            var task = tasks.FirstOrDefault(task => task.Id == id);
            if (task.state == Task.TaskState.Active)
            {
                task.state = Task.TaskState.Complete;
            }
        }
    }
}