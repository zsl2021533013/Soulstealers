using System.Collections.Generic;
using GameFramework.Procedure;
using GameMain.Scripts.Game;
using UnityEngine;
using UnityGameFramework.Runtime;

using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain.Scripts.Procedure
{
    public class ProcedureMain : ProcedureBase
    {
        private GameBase currentGame = null;
        
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            currentGame = new SoulstealersGame();
            currentGame.Initialize();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (currentGame != null)
            {
                currentGame.Shutdown();
                currentGame = null;
            }

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (currentGame is { GameOver: false })
            {
                currentGame.Update(elapseSeconds, realElapseSeconds);
            }
        }
    }
}
