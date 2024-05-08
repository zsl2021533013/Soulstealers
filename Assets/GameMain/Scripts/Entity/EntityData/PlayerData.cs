using UnityEngine;

namespace GameMain.Scripts.Entity.EntityData
{
    public class PlayerData : EntityData
    {
        [SerializeField]
        private string m_Name = null;
        
        public PlayerData(int entityId, int typeId) : base(entityId, typeId)
        {
        }
        
        /// <summary>
        /// 角色名称。
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }
    }
}