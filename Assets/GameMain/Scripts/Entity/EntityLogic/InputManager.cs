using DG.Tweening;
using GameFramework.Event;
using GameMain.Scripts.Event;
using GameMain.Scripts.Utility;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.AI;
using UnityGameFramework.Runtime;

namespace GameMain.Scripts.Entity.EntityLogic
{
    public class InputManager : Entity
    {
        public enum MouseInteractType
        {
            Ground,
            UI
        }

        [SerializeField] 
        private GameObject moveTarget;

        private Player player;
        private NPC dialogueTarget;
        
        public ReactiveProperty<MouseInteractType> mouseInteractType = new ReactiveProperty<MouseInteractType>();
        public ReactiveProperty<bool> isReady2Dialogue = new ReactiveProperty<bool>();
        
        public Player Player
        {
            get
            {
                if (player == null)
                {
                    var Entity = GameEntry.GetComponent<EntityComponent>();
                    player = Entity.GetEntity(AssetUtility.GetEntityAsset("Player")).Logic as Player;
                }
                return player;
            }
        }
        
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            isReady2Dialogue.Subscribe(value =>
            {
                if (value)
                {
                    var Event = GameEntry.GetComponent<EventComponent>();
                    Event.Subscribe(PlayerArriveEventArgs.EventId, OnPlayerArrive);
                }
                else
                {
                    var Event = GameEntry.GetComponent<EventComponent>();
                    if (Event.Check(PlayerArriveEventArgs.EventId, OnPlayerArrive))
                    {
                        Event.Unsubscribe(PlayerArriveEventArgs.EventId, OnPlayerArrive);
                    }
                }
            });
            
            moveTarget.OnTriggerEnterAsObservable()
                .Subscribe(col =>
                {
                    if(col.transform.CompareTag("Player")) 
                    {
                        moveTarget.SetActive(false);
                    }
                })
                .AddTo(this);
            moveTarget.SetActive(false);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            isReady2Dialogue.Value = false;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            
            UpdateMouse();
        }

        private void UpdateMouse()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isReady2Dialogue.Value = false;
                dialogueTarget = null;
                
                switch (mouseInteractType.Value)
                {
                    case MouseInteractType.Ground:
                        var mousePosition = Input.mousePosition;

                        var ray = Camera.main.ScreenPointToRay(mousePosition);

                        if (Physics.Raycast(ray, out var hit))
                        {
                            if (hit.collider.CompareTag("NPC"))
                            {
                                dialogueTarget = hit.collider.GetComponent<NPC>();
                                Player.SetDestination(dialogueTarget.GetDialoguePoint());

                                isReady2Dialogue.Value = true;

                                moveTarget.transform.position = dialogueTarget.GetDialoguePoint() + 0.1f * Vector3.up;
                                moveTarget.SetActive(true);
                            }

                            if (hit.collider.CompareTag("Ground"))
                            {
                                Player.SetDestination(hit.point);
                                
                                moveTarget.transform.position = hit.point + 0.1f * Vector3.up;
                                moveTarget.SetActive(true);
                            }
                        }

                        break;
                    case MouseInteractType.UI:
                        break;
                    default:
                        break;
                }
            }
        }
        
        private void OnPlayerArrive(object sender, GameEventArgs e)
        {
            var ne = (PlayerArriveEventArgs)e;
            
            Player.transform.DOLookAt(dialogueTarget.transform.position, 1f);
            dialogueTarget.StartDialogue();
        }
    }
}