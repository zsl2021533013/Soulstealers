using QFramework;
using UnityEngine.UI;

namespace GameMain.Scripts.UI
{
    public class MainMenuPanelData : UIPanelData
    {
    }
    
    public class MainMenuPanel : UIPanel
    {
        public Button startGameBtn;

        protected override void OnClose()
        {
        }
    }
}