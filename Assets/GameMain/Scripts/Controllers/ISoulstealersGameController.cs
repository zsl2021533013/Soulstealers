using QFramework;

namespace GameMain.Scripts.Controller
{
    public interface ISoulstealersGameController : IController
    {
        public void OnGameInit();

        public void OnUpdate(float elapse);

        public void OnFixedUpdate(float elapse);

        public void OnGameShutdown();
    }
}