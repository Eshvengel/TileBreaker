using Assets.Scripts.Gameplay.Handlers;
using Assets.Scripts.Gameplay.Handlers.PlayerActions;
using Assets.Scripts.Gameplay.Handlers.PlayerActions.Implementations;
using Assets.Scripts.Gameplay.Tiles;
using Assets.Scripts.ThirdParty;
using Assets.Scripts.ThirdParty.Events;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Gameplay
{
    [RequireComponent(typeof(MeshRenderer))]

    public class Player : MonoBehaviour
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public Vector3 WorldOffset { get; private set; }
        public Transform Transform { get; private set; }
        public MeshRenderer MeshRenderer { get; private set; }
        public IPlayerActionHandler PlayerActionHandler { get; private set; }
        
        public Vector3 WorldPosition
        {
            get => Transform.position;
            set => Transform.position = value + WorldOffset;
        }

        private void Awake()
        {
            Transform = gameObject.transform;
            MeshRenderer = GetComponent<MeshRenderer>();
            PlayerActionHandler = new PlayerActionHandler();
            WorldOffset = new Vector3(0, MeshRenderer.bounds.size.y / 2, 0);

            AddListeners();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        public void SetPosition(ITile tile)
        {
            X = tile.X;
            Y = tile.Y;
            WorldPosition = tile.WorldPosition;
        }
        
        public void RoundRotation()
        {
            Transform.eulerAngles = Vector3Int.RoundToInt(Transform.eulerAngles);
        }

        public bool InProcess()
        {
            return PlayerActionHandler.InProcess;
        }

        public void TryMakeAction(IPlayerAction playerAction)
        {
            PlayerActionHandler.MakeAction(playerAction);
        }

        private void AddListeners()
        {
            EventManager.AddListener<GamePlayStartEvent>(OnGamePlayStart);
            EventManager.AddListener<GamePlayRestartEvent>(OnGamePlayRestart);
        }

        private void RemoveListeners()
        {
            EventManager.RemoveListener<GamePlayStartEvent>(OnGamePlayStart);
            EventManager.RemoveListener<GamePlayRestartEvent>(OnGamePlayRestart);
        }

        private void OnInput(Vector2 direction)
        {

        }

        private void OnGamePlayRestart(GamePlayRestartEvent e)
        {
            X = -1;
            Y = -1;
            WorldPosition = Vector3.right * 100;
            
            //PlayerActionHandler.MakeAction(new PlayerActionDespawn(this, 1f, Ease.Linear));
            
            //PlayerActionHandler.MakeAction(new PlayerActionSpawn(this, e.Field.GetStart(), 1f));
        }

        private void OnGamePlayStart(GamePlayStartEvent e)
        {
            //SetPosition(e.Field.GetStart());
            
            PlayerActionHandler.MakeAction(new PlayerActionSpawn(this, e.Field.GetStart(), 1f));
        }
    }
}