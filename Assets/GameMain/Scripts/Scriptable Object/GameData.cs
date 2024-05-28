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
    public class GameData
    {
        public PlayerData playerData = new PlayerData();
        public Dictionary<string, Dictionary<string, object>> npcDataDic = new Dictionary<string, Dictionary<string, object>>();
        public List<Task> tasks = new List<Task>();
    }
}