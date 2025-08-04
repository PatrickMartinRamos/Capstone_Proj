using System;
using CapstoneProj.MiscSystem;
using UnityEngine;

namespace CapstoneProj.ControlPanelSystem
{
    public class OrderedPair : SingletonBehaviour<OrderedPair>
    {
        public event EventHandler<OnTileCoordinateSetEventArgs> OnTileCoordinateSet;
        public class OnTileCoordinateSetEventArgs : EventArgs
        {
            public Vector2Int TileCoordinate { get; private set; }

            public OnTileCoordinateSetEventArgs(Vector2Int tileCoordinate)
            {
                TileCoordinate = tileCoordinate;
            }
        }
        public event EventHandler<OnCoordinateIsScrollableSetEventArgs> OnCoordinateIsScrollableSet;
        public class OnCoordinateIsScrollableSetEventArgs : EventArgs
        {
            public bool IsScrollable { get; private set; }

            public OnCoordinateIsScrollableSetEventArgs(bool isScrollable)
            {
                IsScrollable = isScrollable;
            }
        }

        [SerializeField] private Vector2Int _tileCoordinate;

        public void SetXTileCoordinate(int xCoordinate)
        {
            _tileCoordinate.x = xCoordinate;
            OnTileCoordinateSet?.Invoke(this, new OnTileCoordinateSetEventArgs(_tileCoordinate));
        }

        public void SetYTileCoordinate(int yCoordinate)
        {
            _tileCoordinate.y = yCoordinate;
            OnTileCoordinateSet?.Invoke(this, new OnTileCoordinateSetEventArgs(_tileCoordinate));
        }

        public void SetCoordinateIsScrollable(bool isScrollable)
            => OnCoordinateIsScrollableSet?.Invoke(this, new OnCoordinateIsScrollableSetEventArgs(isScrollable));
    }
}