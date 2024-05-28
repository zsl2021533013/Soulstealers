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
    
    public class GameData : ScriptableObject
    {
        public PlayerData playerData = new PlayerData();
        public SerializedDictionary<string, string> npcDataDic = new SerializedDictionary<string, string>();
        public List<Task> tasks = new List<Task>();
    }
}