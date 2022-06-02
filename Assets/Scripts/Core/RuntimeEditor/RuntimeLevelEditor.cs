using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Data.Levels;
using Assets.Scripts.Data.TilesData;
using Assets.Scripts.Gameplay;
using Assets.Scripts.Gameplay.Field.FieldBuilder;
using Assets.Scripts.Gameplay.Tiles;
using Assets.Scripts.Core.RuntimeEditor.Grid;
using Assets.Scripts.Core.RuntimeEditor.Brush;
using Assets.Scripts.Utils;
using Gameplay;
using UnityEngine;

namespace Assets.Scripts.Core.RuntimeEditor
{
    public class RuntimeLevelEditor : MonoBehaviour
    {
        public int GridSize = 9;
        public int TileSize = 1;

        public int LevelId;
        public TileType TileType;
        public int JumpPower;
        public int SlidePower;
        public Direction Direction;
        public Player Player;
        public Camera EditorCamera;

        private IGrid _grid;
        private IBrush _brush;
        private IGameFieldBuilder _builder;
        private LevelsRepository _levelsRepository;
        private Dictionary<int, TileData> _tilesData;

        #region Unity

        private void Awake()
        {
            _grid = new EditorGrid();
            _brush = new EditorTileBrush(EditorCamera);
            _tilesData = new Dictionary<int, TileData>();
            _levelsRepository = new LevelsRepository();
            _builder = GetComponent<IGameFieldBuilder>();

            StartCoroutine(_levelsRepository.LoadAllLevels());
        }

        private void Update()
        {
            UpdateInput();
            DrawGraphic(GridSize, TileSize);
        }

        #endregion

        #region Input Handle

        private void UpdateInput()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                if (!_brush.IsBrushOverGrid(GridSize))
                {
                    var tileData = CreateTileDataByType(TileType);
                    CreateTile(tileData);
                }
            }

            if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
            {
                if (!_brush.IsBrushOverGrid(GridSize))
                {
                    DestroyTile(_brush.LogicPositionX, _brush.LogicPositionY);
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab))
                Direction = Direction.Next();

            if (Input.GetKeyDown(KeyCode.Alpha1))
                TileType = TileType.Start;

            if (Input.GetKeyDown(KeyCode.Alpha2))
                TileType = TileType.Common;

            if (Input.GetKeyDown(KeyCode.Alpha3))
                TileType = TileType.Jump;

            if (Input.GetKeyDown(KeyCode.Alpha4))
                TileType = TileType.Slide;
        }

        #endregion

        #region Draw Graphic

        private void DrawGraphic(float gridSize, int tileSize)
        {
            _grid?.Draw(gridSize, tileSize);
            _brush.Draw(gridSize, tileSize);
        }

        #endregion

        private TileData CreateTileDataByType(TileType type)
        {
            var tileLogicPositionX = _brush.LogicPositionX;
            var tileLogicPositionY = _brush.LogicPositionY;

            Vector3 tileWorldPosition = GetTileWorldPosition();

            if (type == TileType.Common)
                return new TileCommonData(TileType, tileLogicPositionX, tileLogicPositionY, tileWorldPosition);

            if (type == TileType.Jump)
                return new TileJumperData(TileType, tileLogicPositionX, tileLogicPositionY, tileWorldPosition, Direction, JumpPower);

            if (type == TileType.Start)
                return new TileStartData(TileType, tileLogicPositionX, tileLogicPositionY, tileWorldPosition);

            if (type == TileType.Slide)
                return new TileSlideData(TileType, tileLogicPositionX, tileLogicPositionY, tileWorldPosition, Direction, SlidePower);

            return new TileCommonData(TileType, tileLogicPositionX, tileLogicPositionY, tileWorldPosition);
        }

        private Vector3 GetTileWorldPosition()
        {
            return new Vector3(_brush.LogicPositionX + (float)TileSize / 2, 0, _brush.LogicPositionY + (float)TileSize / 2);
        }

        private void CreateTile(TileData data)
        {
            if (CanCreateTile(data))
            {
                ITile tile = References.Create(data, _builder.GameField);
                _builder.GameField.Add(tile);
                _tilesData[data.GetHashCode()] = data;
            }
        }

        private void DestroyTile(int x, int y)
        {
            if (_builder.GameField.IsTileExist(x, y))
            {
                var tile = _builder.GameField[x, y];
                var data = tile.GetTileData();
                _builder.GameField.Remove(x, y);
                _tilesData.Remove(data.GetHashCode());
            }
        }

        private bool CanCreateTile(TileData data)
        {
            return data.Type != TileType.None && !_builder.GameField.IsTileExist(data.X, data.Y); //!_fakeGameField.IsTileValid(x, y));
        }

        public void SaveLevel()
        {
            var tilesData = _tilesData.Values.ToArray();
            var level = new Level(LevelId, tilesData);

            _levelsRepository.Save(level);
        }

        public void LoadLevel()
        {
            var level = _levelsRepository.Load(LevelId);

            if (level != null)
            {
                Build(level);

                var tiles = _builder.GameField.GetTiles();

                foreach (var tile in tiles)
                {
                    var data = tile.GetTileData();
                    _tilesData[data.GetHashCode()] = data;
                }

            }
        }

        private void Build(Level level)
        {
            _builder.Build(level);
        }

        public void Clear()
        {
            _builder.GameField.Clear();
            _tilesData.Clear();
        }

        public void Play()
        {
            // var tileStart = _builder.GameField.GetStart();
            //
            // if (tileStart != null)
            // {
            //     Player.SetPosition(tileStart);
            // }
        }

        public void Stop()
        {
            
        }
    }
}
