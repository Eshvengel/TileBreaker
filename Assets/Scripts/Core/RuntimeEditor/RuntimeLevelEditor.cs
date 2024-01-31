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
        private const int GRID_SIZE = 9;
        private const int TILE_SIZE = 1;

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

        private void Awake()
        {
            _grid = new EditorGrid();
            _brush = new EditorTileBrush(EditorCamera);
            _levelsRepository = new LevelsRepository();
            _builder = GetComponent<IGameFieldBuilder>();

            StartCoroutine(_levelsRepository.LoadAllLevels());
        }

        private void Update()
        {
            UpdateInput();
            DrawGraphic(GRID_SIZE, TILE_SIZE);
        }

        private void UpdateInput()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                if (!_brush.IsBrushOverGrid(GRID_SIZE))
                {
                    var tileData = CreateTileDataByType(TileType);
                    CreateTile(tileData);
                }
            }

            if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
            {
                if (!_brush.IsBrushOverGrid(GRID_SIZE))
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
        
        private void DrawGraphic(float gridSize, int tileSize)
        {
            _grid?.Draw(gridSize, tileSize);
            _brush.Draw(gridSize, tileSize);
        }

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
            return new Vector3(_brush.LogicPositionX + (float)TILE_SIZE / 2, 0, _brush.LogicPositionY + (float)TILE_SIZE / 2);
        }

        private void CreateTile(TileData data)
        {
            if (CanCreateTile(data))
            {
                ITile tile = Prefabs.CreateTile(data, _builder.GameField);
                _builder.GameField.Add(tile);
            }
        }

        private void DestroyTile(int x, int y)
        {
            if (_builder.GameField.IsTileExist(x, y))
            {
                var tile = _builder.GameField[x, y];
                var data = tile.GetTileData();
                _builder.GameField.Remove(x, y);
            }
        }

        private bool CanCreateTile(TileData data)
        {
            return data.Type != TileType.None && !_builder.GameField.IsTileExist(data.X, data.Y); //!_fakeGameField.IsTileValid(x, y));
        }

        public void SaveLevel()
        {
            var gameField = _builder.GameField;
            var tiles = gameField.GetTiles();
            var tilesData = new TileData[tiles.Count];

            for (int i = 0; i < tiles.Count; i++)
            {
                tilesData[i] = tiles[i].GetTileData();
            }
            
            var level = new Level(LevelId, tilesData);

            _levelsRepository.Save(level);
        }

        public void LoadLevel()
        {
            Clear();
            
            var level = _levelsRepository.Load(LevelId);

            if (level != null)
            {
                Build(level);

                var tiles = _builder.GameField.GetTiles();

                foreach (var tile in tiles)
                {
                    var data = tile.GetTileData();
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
        }

        public void Play()
        {
            
        }

        public void Stop()
        {
            
        }
    }
}
