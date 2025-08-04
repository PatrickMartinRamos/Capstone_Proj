using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class BombVisual : MonoBehaviour
    {
        [SerializeField] private Bomb _bomb;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_bomb != null)
                _bomb.OnBombSpawned
                    += Bomb_OnBombSpawned;
        }

        private void OnDestroy()
        {
            if (_bomb != null)
            {
                _bomb.OnBombSpawned
                    -= Bomb_OnBombSpawned;
            }
        }

        private void Bomb_OnBombSpawned(object sender, Bomb.OnBombSpawnedEventArgs e)
            => _spriteRenderer.sprite = e.BombSprite;
    }
}