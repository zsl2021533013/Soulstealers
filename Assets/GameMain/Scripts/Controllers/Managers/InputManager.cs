using System.Linq;
using DG.Tweening;
using EPOOutline;
using GameMain.Scripts.Controller;
using GameMain.Scripts.Event;
using GameMain.Scripts.Model;
using GameMain.Scripts.Utility;
using QFramework;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMain.Scripts.Entity.EntityLogic
{
    public class InputManager : MonoSingleton<InputManager>, ISoulstealersGameController
    {
        public enum MouseInteractType
        {
            Ground,
            Dialogue,
            UI
        }

        [SerializeField] 
        private GameObject moveTarget;

        private PlayerController playerController;
        private NPCController dialogueTarget;
        
        public ReactiveProperty<MouseInteractType> mouseInteractType = new ReactiveProperty<MouseInteractType>();
        public ReactiveProperty<bool> isReady2Dialogue = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> isShowingOutline = new ReactiveProperty<bool>();

        private void Awake()
        {
            isReady2Dialogue.Subscribe(value =>
            {
                if (value)
                {
                    this.RegisterEvent<PlayerArriveEvent>(OnPlayerArrive).UnRegisterWhenDisabled(this);
                }
                else
                {
                    this.UnRegisterEvent<PlayerArriveEvent>(OnPlayerArrive);
                }
            });

            isShowingOutline.Subscribe(value =>
            {
                if (value)
                {
                    var Outlinables = FindObjectsOfType<Outlinable>();
                    foreach (var outlinable in Outlinables)
                    {
                        outlinable.enabled = true;
                    }
                }
                else
                {
                    var Outlinables = FindObjectsOfType<Outlinable>();
                    foreach (var outlinable in Outlinables)
                    {
                        outlinable.enabled = false;
                    }
                }
            });
            
            moveTarget.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    var cols = Physics.OverlapSphere(moveTarget.transform.position, 0.5f);
                    if (cols.Any(col => col.CompareTag("Player")))
                    {
                        moveTarget.SetActive(false);
                    }
                })
                .AddTo(moveTarget);
            moveTarget.SetActive(false);
        }

        public void OnGameInit()
        {
            playerController = this.GetModel<PlayerModel>().controller;
        }

        public void OnUpdate(float elapse)
        {
            UpdateMouse();
        }

        public void OnFixedUpdate(float elapse)
        {
            
        }

        public void OnGameShutdown()
        {
            isReady2Dialogue.Value = false;
        }

        private void UpdateMouse()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
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
                                dialogueTarget = hit.collider.GetComponent<NPCController>();
                                playerController.SetDestination(dialogueTarget.GetDialoguePoint());

                                isReady2Dialogue.Value = true;

                                moveTarget.transform.position = dialogueTarget.GetDialoguePoint() + 0.1f * Vector3.up;
                                moveTarget.SetActive(true);
                            }

                            if (hit.collider.CompareTag("Ground"))
                            {
                                playerController.SetDestination(hit.point);
                                
                                moveTarget.transform.position = hit.point + 0.1f * Vector3.up;
                                moveTarget.SetActive(true);
                            }
                        }

                        break;
                    case MouseInteractType.Dialogue:
                        break;
                    default:
                        break;
                }
            }
            
            if (Input.GetMouseButton(1))
            {
                isShowingOutline.Value = true;
            }
            else
            {
                isShowingOutline.Value = false;
            }
        }
        
        private void OnPlayerArrive(PlayerArriveEvent e)
        {
            var player = this.GetModel<PlayerModel>().transform;
            player.DOLookAt(dialogueTarget.transform.position, 1f);
            dialogueTarget.StartDialogue();
        }

        public IArchitecture GetArchitecture()
        {
            return Soulstealers.Interface;
        }
    }
}