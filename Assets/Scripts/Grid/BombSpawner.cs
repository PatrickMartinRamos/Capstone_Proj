using System;
using System.Collections.Generic;
using CapstoneProj.CoreSystem;
using CapstoneProj.EnumSystem;
using CapstoneProj.MiscSystem;
using CapstoneProj.ScriptableObjectSystem;
using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class BombSpawner : MonoBehaviour
    {
        [SerializeField] private BombListSO _bombListSO;

        private void Start()
        {
            BombSpawnTimer.Instance.OnBombSpawnTimerRunOut
                += BombSpawnTimer_OnBombSpawnTimerRunOut;
        }

        private void OnDestroy()
        {
            BombSpawnTimer.Instance.OnBombSpawnTimerRunOut
                -= BombSpawnTimer_OnBombSpawnTimerRunOut;
        }

        private void BombSpawnTimer_OnBombSpawnTimerRunOut(object sender, EventArgs e)
        {
            StageSO stageSO = World2GameManager.Instance.GetStageSO();
            List<BombType> bombTypeList = Utils.GetSelectedFlags(stageSO.BombType);

            int bombTypeListIndex = UnityEngine.Random.Range(0, bombTypeList.Count);
            BombType bombType = bombTypeList[bombTypeListIndex];

            BombSO[] bombSOArray = _bombListSO.BombSOList.FindAll(bombSOListElement => bombSOListElement.BombType == bombType).ToArray();

            int bombSOArrayIndex = UnityEngine.Random.Range(0, bombSOArray.Length);
            BombSO bombSO = bombSOArray[bombSOArrayIndex];

            int tileListIndex = -1;
            List<Tile> tileList = TileSpawner.Instance.GetTileList();

            while (tileListIndex == -1)
            {
                tileListIndex = UnityEngine.Random.Range(0, tileList.Count);

                if (tileList[tileListIndex].HasBomb())
                {
                    tileListIndex = -1;
                    continue;
                }
                else
                    break;
            }

            Tile tile = tileList[tileListIndex];

            Bomb.SpawnBomb(bombSO, tile);
        }
    }
}