using System;
using CapstoneProj.EnumSystem;
using CapstoneProj.ScriptableObjectSystem;
using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class Bomb : MonoBehaviour
    {
        public event EventHandler<OnBombSpawnedEventArgs> OnBombSpawned;
        public class OnBombSpawnedEventArgs : EventArgs
        {
            public Sprite BombSprite { get; private set; }

            public OnBombSpawnedEventArgs(Sprite bombSprite)
            {
                BombSprite = bombSprite;
            }
        }

        [SerializeField] private BombSO _bombSO;

        private Tile _parentTile;

        public void SetBombSO(BombSO bombSO)
            => _bombSO = bombSO;

        public BombSO GetBombSO()
            => _bombSO;

        public void SetParentTile(Tile parentTile)
        {
            if (_parentTile != null)
                _parentTile.ClearBomb();

            if (parentTile.HasBomb())
                Debug.LogError("Tile already has a Bomb");

            _parentTile = parentTile;
            parentTile.SetBomb(this);
            transform.parent = parentTile.GetTileTransform();
            transform.localPosition = Vector3.zero;
        }

        public Tile GetParentTile()
            => _parentTile;

        public void DestroySelf()
        {
            _parentTile.ClearBomb();

            Destroy(gameObject);
        }

        public static Bomb SpawnBomb(BombSO bombSO, Tile parentTile, BombAnimationType bombAnimationType)
        {
            Transform bombTransform = Instantiate(bombSO.BombPrefab, parentTile.GetTileTransform());

            if (bombTransform.TryGetComponent(out Bomb bomb))
            {
                bomb.SetBombSO(bombSO);
                bomb.SetParentTile(parentTile);
    
                if (bomb.TryGetComponent(out BombAnimation bombAnimation))
                    bombAnimation.SetBombAnimationType(bombAnimationType);

                bomb.OnBombSpawned?.Invoke(bomb, new OnBombSpawnedEventArgs(bombSO.BombSprite));
                
                return bomb;
            }

            return null;
        }
    }
}