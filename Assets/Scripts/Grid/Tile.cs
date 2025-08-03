using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private Bomb _bomb;

        public Transform GetTileTransform()
            => transform;

        public void SetBomb(Bomb bomb)
            => _bomb = bomb;

        public Bomb GetBomb()
            => _bomb;

        public void ClearBomb()
            => _bomb = null;

        public bool HasBomb()
            => _bomb != null;
    }
}