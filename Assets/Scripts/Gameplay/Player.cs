using Assets.Scripts.Gameplay.Field;
using Assets.Scripts.Gameplay.Field.FieldBuilder;
using Assets.Scripts.Gameplay.Handlers;
using Assets.Scripts.Gameplay.Handlers.PlayerActions;
using Assets.Scripts.Gameplay.Handlers.PlayerActions.Implementations;
using Assets.Scripts.Gameplay.Tiles;
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
        
        public Vector3 WorldPosition
        {
            get => Transform.position;
            set => Transform.position = value + WorldOffset;
        }
        
        private IPlayerActionHandler _playerActionHandler;
        private InputHandler _inputHandler;

        private void Awake()
        {
            Transform = gameObject.transform;
            MeshRenderer = GetComponent<MeshRenderer>();
            WorldOffset = new Vector3(0, MeshRenderer.bounds.size.y / 2, 0);
        }

        public void Initialize(GameField gameField)
        {
            _playerActionHandler = new PlayerActionHandler();
            _inputHandler = new InputHandler(this, gameField);
            
            TryMakeAction(new PlayerActionSpawn(this, gameField.GetStart()));
        }
        

        private void Update()
        {
            _inputHandler.Update();
        }

        public void SetPosition(ITile tile)
        {
            X = tile.X;
            Y = tile.Y;
            WorldPosition = tile.WorldPosition;
        }
        
        public void RoundRotation()
        {
            Transform.rotation = Quaternion.Euler(Vector3Int.RoundToInt(Transform.eulerAngles));
        }

        public bool InProcess()
        {
            return _playerActionHandler.InProcess;
        }

        public void TryMakeAction(IPlayerAction playerAction)
        {
            _playerActionHandler.MakeAction(playerAction);
        }

        public void Destroy()
        {
            _playerActionHandler.Dispose();
            
            Destroy(gameObject);
        }
    }
}