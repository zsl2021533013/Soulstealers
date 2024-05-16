using GameFramework.Event;
using GameFramework.Procedure;
using GameMain.Scripts.UI;
using UnityEngine;
using UnityGameFramework.Runtime;

using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain.Scripts.Procedure
{
    public class ProcedureMenu : ProcedureBase
    {
        private bool startGame = false;
        private MenuForm menuForm = null;

        public void StartGame()
        {
            startGame = true;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            
            var Event = GameEntry.GetComponent<EventComponent>();
            var UI = GameEntry.GetComponent<UIComponent>();

            Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            startGame = false;
            UI.OpenUIForm(UIFormId.MenuForm, this);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            
            var Event = GameEntry.GetComponent<EventComponent>();

            Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            if (menuForm != null)
            {
                menuForm.Close(isShutdown);
                menuForm = null;
            }
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (startGame)
            {
                var Config = GameEntry.GetComponent<ConfigComponent>();
                procedureOwner.SetData<VarInt32>("NextSceneId", Config.GetInt("Scene.Main"));
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            if (ne.UIForm.Logic is MenuForm form)
            {
                menuForm = form;
            }
        }
    }
}