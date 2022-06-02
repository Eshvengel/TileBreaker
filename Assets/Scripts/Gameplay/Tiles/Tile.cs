using System.Security.Cryptography;
using Assets.Scripts.Data.TilesData;
using Assets.Scripts.Gameplay.Field;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Tiles
{
    public abstract class Tile<T> : MonoBehaviour, ITile where T : TileData
    {
        private const float SHOW_DURATION = 0.4f;
        private const float HIDE_DURATION = 0.4f;
        
        public T Data { get; private set; }
        public bool Interactable { get; protected set; }
        public Sequence AnimationSequence { get; protected set; }

        public int X => Data.X;
        public int Y => Data.Y;
        public Vector3 WorldPosition => Data.WorldPosition; 
        
        public abstract void OnPlayerExit(Player player);
        public abstract void OnPlayerEnter(Player player);
        public TileData GetTileData()
        {
            return Data;
        }

        protected virtual void OnInitialize()
        {
            
        }

        protected GameField GameField { get; private set; }

        public void Initialize(T data, GameField gameField)
        {
            Data = data;
            GameField = gameField;
            AnimationSequence = DOTween.Sequence();

            OnInitialize();
        }
        
        public void Show()
        {
            if (AnimationSequence != null)
            {
                AnimationSequence.Kill();
                AnimationSequence = null;
                AnimationSequence = DOTween.Sequence();
            }

            Interactable = true;

            transform.localPosition += Vector3.down * 3;
            transform.localScale = Vector3.zero;

            AnimationSequence
                .Insert(0, transform.DOScale(new Vector3(1, 0.1f, 1), SHOW_DURATION))
                .Insert(0, transform.DOMove(WorldPosition, SHOW_DURATION).SetEase(Ease.OutBack));
        }

        public void Hide()
        {
            if (AnimationSequence != null)
            {
                AnimationSequence.Kill();
                AnimationSequence = null;
                AnimationSequence = DOTween.Sequence();
            }

            Interactable = false;

            AnimationSequence
                .Insert(0, transform.DOScale(Vector3.zero, HIDE_DURATION))
                .Insert(0, transform.DOMove(transform.localPosition + Vector3.down * 3, HIDE_DURATION));
        }

        public bool Equal(int x, int y)
        {
            return X == x && Y == y;
        }

        public bool Equal(ITile tile)
        {
            return X == tile.X && Y == tile.Y;
        }

        public void Destroy()
        {
            AnimationSequence.Kill();
            Destroy(gameObject);
        }
    }
}
