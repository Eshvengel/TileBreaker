using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Gameplay.Tiles;
using Assets.Scripts.Gameplay.Tiles.Implementations;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Field
{
    public class GameField : IEnumerable
    {
        private const int MIN_TILES_COUNT = 1;
        private const string GAMEFIELD_NAME = "GameField";

        private List<ITile> _tiles;
        private readonly Transform _tilesContainer;
        
        public GameField()
        {
            _tiles = new List<ITile>();
            _tilesContainer = new GameObject(GAMEFIELD_NAME).transform;
        }

        public ITile this[int x, int y]
        {
            get
            {
                return _tiles.FirstOrDefault(tile => tile.Equal(x, y));
            }
        }

        public void Add(ITile tile)
        {
            if (_tiles == null)
                _tiles = new List<ITile>();
            
            _tiles.Add(tile);
        }

        public void Remove(ITile tile)
        {
            if (_tiles == null)
            {
                Debug.LogWarning("Tiles were not initialized!");
                return;
            }

            tile.Destroy();
            _tiles.Remove(tile);
        }

        public void Remove(int x, int y)
        {
            if (_tiles == null)
            {
                Debug.LogWarning("Tiles were not initialized!");
                return;
            }

            var tile = _tiles.Find(t => t.Equal(x, y));

            if (tile != null)
            {
                tile.Destroy();
                _tiles.Remove(tile);
            }
        }

        public bool HasMoves(Player player)
        {
            return HasNeighbor(player.X, player.Y);
        }

        public bool IsClean()
        {
            DestroyNullTiles();
            
            return _tiles.Count(cell => cell.Interactable) <= MIN_TILES_COUNT;
        }

        public bool IsTileExist(int x, int y)
        {
            return _tiles.Any(tile => tile.Equal(x, y));
        }

        public TileStart GetStart()
        {
            return (TileStart) _tiles.FirstOrDefault(tile => tile is TileStart);
        }
        
        public void Clear()
        {
            foreach (var tile in _tiles)
            {
                tile.Destroy();
            }
            
            _tiles.Clear();
        }

        public Transform GetGameFieldContainer()
        {
            return _tilesContainer;
        }
        
        public List<ITile> GetTiles()
        {
            return _tiles;
        }

        public IEnumerator GetEnumerator()
        {
            return _tiles.GetEnumerator();
        }

        private bool HasNeighbor(int x, int y)
        {
            // Right
            if (this[x + 1, y] != null && this[x + 1, y].Interactable)
                return true;

            // Left
            if (this[x - 1, y] != null && this[x - 1, y].Interactable)
                return true;

            // Up
            if (this[x, y + 1] != null && this[x, y + 1].Interactable)
                return true;

            // Down
            if (this[x, y - 1] != null && this[x, y - 1].Interactable)
                return true;

            return false;
        }

        private void DestroyNullTiles()
        {
            _tiles.RemoveAll(tile => tile == null);
        }
    }
}
