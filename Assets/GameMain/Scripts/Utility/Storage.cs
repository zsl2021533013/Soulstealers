using UnityEngine;

namespace GameMain.Scripts.Utility
{
    public class Storage
    {
        public string Save(object o)
        {
            return JsonUtility.ToJson(o);
        }

        public void Load<T>(ref object c, string json)
        {
            c = JsonUtility.FromJson<T>(json);
        }
    }
}