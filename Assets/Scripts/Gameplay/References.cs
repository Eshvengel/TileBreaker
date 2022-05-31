using Assets.Scripts.Data.TilesData;
using Assets.Scripts.Gameplay;
using Assets.Scripts.Gameplay.Field;
using Assets.Scripts.Gameplay.Tiles;
using Assets.Scripts.Gameplay.Tiles.Implementations;
using UnityEngine;

namespace Gameplay
{
    public class References : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private TileJump _tileJump;
        [SerializeField] private TileSlide _tileSlide;
        [SerializeField] private TileStart _tileStart;
        [SerializeField] private TileCommon _tileCommon;

//        [SerializeField] 
//        private TileSolid _tileSolid;

//        [SerializeField] 
//        private TilePortal _tilePortal;

        private static References instance;

        private void Awake()
        {
            instance = this;
        }

        public static Player CreatePlayer()
        {
            return instance.Get();
        }
        
        private Player Get()
        {
            return Instantiate(_player);
        }
        
        
        public static ITile Create(TileData data, GameField gameField)
        {
            return instance.Get(data, gameField);
        }

        private ITile Get(TileData data, GameField gameField)
        {
            var type = data.Type;
            var position = data.WorldPosition;
            var parent = gameField.GetGameFieldContainer();

            if (type == TileType.Common)
            {
                var tile = Instantiate(_tileCommon, position, Quaternion.identity, parent);
                tile.Initialize(new TileCommonData(data.Type, data.X, data.Y, data.WorldPosition), gameField);
                return tile;
            }

            if (type == TileType.Start)
            {
                var tile = Instantiate(_tileStart, position, Quaternion.identity, parent);
                tile.Initialize(new TileStartData(data.Type, data.X, data.Y, data.WorldPosition), gameField);
                return tile;
            }

            if (type == TileType.Jump)
            {
                var tile = Instantiate(_tileJump, position, Quaternion.identity, parent);
                tile.Initialize(new TileJumperData(data.Type, data.X, data.Y, data.WorldPosition, data.JumpDirection, data.JumpPower), gameField);
                return tile;
            }

            if (type == TileType.Slide)
            {
                var tile = Instantiate(_tileSlide, position, Quaternion.identity, parent);
                tile.Initialize(new TileSlideData(data.Type, data.X, data.Y, data.WorldPosition, data.SlideDirection, data.SlidePower), gameField);
                return tile;
            }
      
            Debug.LogError($"Type \"{ type }\" is not define!");
            
            return null;
        }
    }
}
