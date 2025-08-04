using System.Collections.Generic;
using CapstoneProj.EnumSystem;
using CapstoneProj.MiscSystem;
using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class BottomScreenBombSpawner : SingletonBehaviour<BottomScreenBombSpawner>
    {
        [SerializeField] private Tile _originalTopTile;

        public void SpawnBomb(Tile topTile)
        {
            _originalTopTile = topTile; // Store the original top tile for reference.

            Bomb bomb = topTile.GetBomb();

            List<Tile> bottomTileList = BottomScreenTileSpawner.Instance.GetBottomTileList();

            if (bottomTileList.Count == 0)
            {
                Debug.LogError("No bottom tiles available to spawn a bomb.");
                return;
            }

            int bottomTileIndex = Random.Range(0, bottomTileList.Count);
            Tile bottomTile = bottomTileList[bottomTileIndex];

            Bomb.SpawnBomb(bomb.GetBombSO(), bottomTile, BombAnimationType.BombMarker);
        }
    }
}