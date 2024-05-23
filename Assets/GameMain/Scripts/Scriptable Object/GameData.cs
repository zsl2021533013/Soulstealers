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
        public Dictionary<string, string> dialogueData = new Dictionary<string, string>();
        public List<Task> tasks = new List<Task>();
    }
}