using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace GameMain.Scripts.Game
{
    public abstract class GameBase
    {
        public bool GameOver
        {
            get;
            protected set;
        }
        
        public virtual void Initialize()
        {
            var Event = GameEntry.GetComponent<EventComponent>();
            
            Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);

            

            GameOver = false;
        }

        public virtual void Shutdown()
        {
            var Event = GameEntry.GetComponent<EventComponent>();
            
            Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
        }

        public virtual void Update(float elapseSeconds, float realElapseSeconds)
        {
            
        }

        protected virtual void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            
        }

        protected virtual void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs)e;
            Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
        }
    }
}