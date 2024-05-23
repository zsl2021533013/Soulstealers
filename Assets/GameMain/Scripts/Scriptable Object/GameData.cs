using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameMain.Scripts.Scriptable_Object
{
    [Serializable]
    public class PlayerData
    {
        public Vector3 position;
        public Quaternion rotation;
    }
    
    public class GameData : ScriptableObject
    {
        public PlayerData playerData = new PlayerData();
        public Dictionary<Blackboard, string> dialogueData = new Dictionary<Blackboard, string>();
        public TaskData taskData;
    }
}