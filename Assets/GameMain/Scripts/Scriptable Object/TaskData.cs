using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameMain.Scripts.Scriptable_Object
{
    [Serializable]
    public class Task
    {
        public enum TaskState
        {
            Inactive,
            Active,
            Complete
        }

        public Task(int id)
        {
            this.id = id;
        }

        [SerializeField] private int id;
        public int Id => id;
        
        public TaskState state;
        public string activeText;
        public string completeText;
    }
    
    [CreateAssetMenu(fileName = "QuestsData", menuName = "Scriptable Object/QuestsData")]
    public class TaskData : ScriptableObject
    {
        [ListDrawerSettings(CustomAddFunction = "AddTask")]
        public List<Task> tasks;
        
        private Task AddTask()
        {
            return new Task(tasks.Count);
        }
    }
}