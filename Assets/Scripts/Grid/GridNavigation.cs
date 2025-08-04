using System.Collections.Generic;
using CapstoneProj.ControlPanelSystem;
using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class GridNavigation : MonoBehaviour
    {
        private void Start()
            => OrderedPair.Instance.OnTileCoordinateSet
                += OrderedPair_OnTileCoordinateSet;

        private void OrderedPair_OnTileCoordinateSet(object sender, OrderedPair.OnTileCoordinateSetEventArgs e)
        {
            List<Tile> bottomTileList = BottomScreenTileSpawner.Instance.GetBottomTileList();

            Tile bottomTile = bottomTileList.Find(bottomtTileListElement => bottomtTileListElement.GetTileCoordinates() == e.TileCoordinate);

            if (bottomTile == null)
                return;

            GridNavigator.Instance.SetParentTile(bottomTile);
        }
    }
}