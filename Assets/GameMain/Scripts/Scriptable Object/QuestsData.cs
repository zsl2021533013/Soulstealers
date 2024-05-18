using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain.Scripts.Scriptable_Object
{
    [Serializable]
    public class Quest
    {
        private static int serialId = 0;
        
        public enum QuestState
        {
            Inactive,
            Active,
            Complete
        }

        public Quest()
        {
            id = ++serialId;
        }
        
        
        [SerializeField] private int id;

        public int Id => id;
        
        public QuestState state;
        public string activeText;
        public string completeText;
    }
    
    [CreateAssetMenu(fileName = "QuestsData", menuName = "Scriptable Object/QuestsData", order = 0)]
    public class QuestsData : ScriptableObject
    {
        public List<Quest> quests;
    }
}