using System.Collections.Generic;
using System.Linq;
using GameMain.Scripts.Event;
using GameMain.Scripts.Game;
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
            var data = ES3.Load<GameData>(SoulstealersGame.DataName, 
                AssetUtility.GetSaveAsset(SoulstealersGame.DataName)).tasks;

            tasks = new List<Task>(data);
        }

        public void ActivateTask(int id)
        {
            var task = tasks.FirstOrDefault(task => task.Id == id);
            if (task.state == Task.TaskState.Inactive)
            {
                task.state = Task.TaskState.Active;
            }
            
            this.SendEvent<ModelChangeEvent>();
        }

        public void CompleteTask(int id)
        {
            var task = tasks.FirstOrDefault(task => task.Id == id);
            if (task.state == Task.TaskState.Active)
            {
                task.state = Task.TaskState.Complete;
            }
            
            this.SendEvent<ModelChangeEvent>();
        }
    }
}