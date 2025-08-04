using System.Collections.Generic;
using System.Linq;
using CapstoneProj.CoreSystem;
using CapstoneProj.EnumSystem;
using CapstoneProj.MiscSystem;
using CapstoneProj.ScriptableObjectSystem;
using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class TopScreenBombSpawner : SingletonBehaviour<TopScreenBombSpawner>
    {
        [SerializeField] private BombListSO _bombListSO;
        private List<Tile> _topTileList;
        private bool _isGridFull;

        public void SpawnInitialBombs(List<Tile> topTileList)
        {
            int initialBombCount = World2GameManager.Instance.GetStageSO().InitialBombCount;
            _topTileList = topTileList;

            for (int i = 0; i < initialBombCount; i++)
            {
                SpawnBomb();
            }
        }

        public void SpawnBomb()
        {
            _isGridFull = _topTileList.All(tile => tile.HasBomb());

            if (_isGridFull)
            {
                // TODO: Lose State
                Debug.Log("Grid is full, cannot spawn more bombs.");
                Debug.Log("INCLUDE LOSE STATE HERE");
                BombSpawnTimer.Instance.StopCountdown();
                return;
            }

            StageSO stageSO = World2GameManager.Instance.GetStageSO();
            List<BombType> bombTypeList = Utils.GetSelectedFlags(stageSO.BombType);

            if (bombTypeList.Count == 0)
            {
                Debug.LogError("No Bomb Types selected in StageSO, cannot spawn bomb.");
                return;
            }

            int bombTypeListIndex = Random.Range(0, bombTypeList.Count);
            BombType bombType = bombTypeList[bombTypeListIndex];

            BombSO[] bombSOArray = _bombListSO.BombSOList.FindAll(bombSOListElement => bombSOListElement.BombType == bombType).ToArray();

            int bombSOArrayIndex = Random.Range(0, bombSOArray.Length);
            BombSO bombSO = bombSOArray[bombSOArrayIndex];

            int tileListIndex = -1;

            while (tileListIndex == -1)
            {
                tileListIndex = Random.Range(0, _topTileList.Count);

                if (_topTileList[tileListIndex].HasBomb())
                {
                    tileListIndex = -1;
                    continue;
                }
                else
                    break;
            }

            Tile topTile = _topTileList[tileListIndex];

            Bomb.SpawnBomb(bombSO, topTile, BombAnimationType.Bomb);
        }
    }
}