using GameFramework.Fsm;
using GameFramework.Procedure;

namespace GameMain.Scripts.Procedure
{
    public class ProcedureLaunch : ProcedureBase
    {
        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        
            ChangeState<ProcedurePreload>(procedureOwner);
        }
    }
}
