using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace GameMain.Scripts.Scriptable_Object
{
    [Serializable]
    public class PlayerData
    {
        public Vector3 position;
        public Quaternion rotation;
    }

    [Serializable]
    public class NPCData
    {
        public Dictionary<string, Dictionary<int, bool>> dialogueTreeData;
        public Dictionary<string, Variable> blackboardData;
        public bool active;
        public Vector3 position;
        public Quaternion rotation;
        public string tag;
    }
    
    [Serializable]
    public class GameData
    {
        public PlayerData playerData = new PlayerData();
        public Dictionary<string, NPCData> npcDataDic = new Dictionary<string, NPCData>();
        public List<Task> tasks = new List<Task>();
    }
}