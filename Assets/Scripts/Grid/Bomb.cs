using CapstoneProj.ScriptableObjectSystem;
using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class Bomb : MonoBehaviour
    {
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
                Debug.LogError("Tile already has a Bomb"); // TODO: Change Functionality

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

        public static Bomb SpawnBomb(BombSO bombSO, Tile parentTile)
        {
            Transform bombTransform = Instantiate(bombSO.BombPrefab, parentTile.GetTileTransform());

            if (bombTransform.TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                if (spriteRenderer.sprite != bombSO.BombSprite)
                    spriteRenderer.sprite = bombSO.BombSprite;
            }

            if (bombTransform.TryGetComponent(out Bomb bomb))
            {
                bomb.SetBombSO(bombSO);
                bomb.SetParentTile(parentTile);
                return bomb;
            }

            return null;
        }
    }
}