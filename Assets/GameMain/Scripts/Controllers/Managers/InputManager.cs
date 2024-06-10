using System.Linq;
using DG.Tweening;
using EPOOutline;
using GameMain.Scripts.Controller;
using GameMain.Scripts.Event;
using GameMain.Scripts.Model;
using QFramework;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using AssetUtility = GameMain.Scripts.Utility.AssetUtility;

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

        public enum CursorType
        {
            Normal,
            Select
        }

        [SerializeField] 
        private GameObject moveTarget;

        private PlayerController playerController;
        private NPCController dialogueTarget;

        private Texture2D cursorNormal;
        private Texture2D cursorSelect;
        
        public ReactiveProperty<MouseInteractType> mouseInteractType;
        public ReactiveProperty<CursorType> cursorType;
        public ReactiveProperty<bool> isReady2Dialogue;
        public ReactiveProperty<bool> isShowingOutline;

        public void OnGameInit()
        {
            mouseInteractType = new ReactiveProperty<MouseInteractType>(MouseInteractType.Ground);
            cursorType = new ReactiveProperty<CursorType>(CursorType.Normal);
            isReady2Dialogue = new ReactiveProperty<bool>(false);
            isShowingOutline = new ReactiveProperty<bool>(false);
            
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
                var playerModel = this.GetModel<PlayerModel>();
                var npcModel = this.GetModel<NPCModel>();
                var player = playerModel.transform;
                var NPCs = npcModel.NPCs;
                
                if (value)
                {
                    NPCs.ForEach(npc =>
                    {
                        if (npc.CompareTag("NPC") && Vector3.Distance(npc.transform.position, player.position) < 10f)
                        {
                            npc.EnableOutline();
                        }
                    });
                }
                else
                {
                    NPCs.ForEach(npc =>
                    {
                        if (npc.CompareTag("NPC"))
                        {
                            npc.DisableOutline();
                        }
                    });
                }
            });

            cursorType.Subscribe(value =>
            {
                if (value == CursorType.Select)
                {
                    Cursor.SetCursor(cursorSelect, Vector2.zero, CursorMode.Auto);
                }
                else
                {
                    Cursor.SetCursor(cursorNormal, Vector2.zero, CursorMode.Auto);
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
            
            playerController = this.GetModel<PlayerModel>().controller;

            cursorNormal = Resources.Load<Texture2D>(AssetUtility.GetCursorAsset("Normal"));
            cursorSelect = Resources.Load<Texture2D>(AssetUtility.GetCursorAsset("Select"));
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
            var mousePosition = Input.mousePosition;
            var ray = Camera.main.ScreenPointToRay(mousePosition);
            Physics.Raycast(ray, out var hit);
            
            if (hit.collider && hit.collider.CompareTag("NPC"))
            {
                cursorType.Value = CursorType.Select;
            }
            else
            {
                cursorType.Value = CursorType.Normal;
            }
            
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                isReady2Dialogue.Value = false;
                dialogueTarget = null;
                
                switch (mouseInteractType.Value)
                {
                    case MouseInteractType.Ground:
                        if (hit.collider)
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
                if (mouseInteractType.Value == MouseInteractType.Ground)
                {
                    isShowingOutline.Value = true;
                }
                if (mouseInteractType.Value == MouseInteractType.Dialogue)
                {
                    DialogueManager.Instance.SkipDialogue();
                }
            }
            else
            {
                isShowingOutline.Value = false;
            }
        }
        
        private void OnPlayerArrive(PlayerArriveEvent e)
        {
            DialogueManager.Instance.StartDialogue(dialogueTarget);
        }

        public IArchitecture GetArchitecture()
        {
            return Soulstealers.Interface;
        }
    }
}