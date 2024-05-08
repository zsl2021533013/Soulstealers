using UnityEngine;

namespace GameMain.Scripts.Entity.EntityData
{
    public class NPCData : EntityData
    {
        [SerializeField]
        private string m_Name = null;
        
        public NPCData(int entityId, int typeId) : base(entityId, typeId)
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