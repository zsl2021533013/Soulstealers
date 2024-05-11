using GameFramework;
using GameFramework.Event;

namespace GameMain.Scripts.Event
{
    public class PlayerArriveEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PlayerArriveEventArgs).GetHashCode();
        
        public override void Clear() { }

        public static PlayerArriveEventArgs Create()
        {
            var playerArriveEventArgs = ReferencePool.Acquire<PlayerArriveEventArgs>();
            return playerArriveEventArgs;
        }

        public override int Id => EventId;
    }
}