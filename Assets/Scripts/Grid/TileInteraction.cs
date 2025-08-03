using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class TileInteraction : MonoBehaviour
    {
        [SerializeField] private Tile _tile;

        private void Awake()
        {
            if (_tile == null)
            {
                if (_tile.TryGetComponent(out Tile tile))
                    _tile = tile;
            }
        }

        public void OnInteracted()
        {
            if (_tile.HasBomb())
                Debug.Log($"Tile Detected: {name} has a {_tile.GetBomb().name}!");
        }
    }
}