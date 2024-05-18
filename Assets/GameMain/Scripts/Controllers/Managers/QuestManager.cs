using GameMain.Scripts.Controller;
using GameMain.Scripts.UI;
using QFramework;

namespace GameMain.Scripts.Entity.EntityLogic
{
    public class QuestManager : MonoSingleton<QuestManager>, ISoulstealersGameController
    {
        public void OnGameInit()
        {
        }

        public void OnUpdate(float elapse)
        {
        }

        public void OnFixedUpdate(float elapse)
        {
        }

        public void OnGameShutdown()
        {
        }
        
        public IArchitecture GetArchitecture()
        {
            return Soulstealers.Interface;
        }
    }
}