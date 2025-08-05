using System;
using CapstoneProj.ControlPanelSystem;
using CapstoneProj.CoreSystem;
using CapstoneProj.GridSystem;
using CapstoneProj.ScriptableObjectSystem;
using UnityEngine;

namespace CapstoneProj.ProgressSystem
{
    public class Progression : MonoBehaviour
    {
        [SerializeField] private int _points = 0;

        public void ResetProgression()
        {
            _points = 0;
        }

        private void Awake()
        {
            ResetProgression();
        }

        private void Start()
        {
            DefuseButton.Instance.OnClick
                += DefuseButton_OnClick;
        }

        private void DefuseButton_OnClick(object sender, EventArgs e)
        {
            Tile tile = GridNavigator.Instance.GetParentTile();

            if (tile.HasBomb())
            {
                DefuseButton.Instance.CorrectClick();
                Bomb bomb = tile.GetBomb();
                _points += bomb.GetBombSO().CorrectPoints;
                StageSO stageSO = World2GameManager.Instance.GetStageSO();
                ProgressBar.Instance.Progress(_points / (float) stageSO.RequiredPoints);
            }
            else
                DefuseButton.Instance.IncorrectClick();
        }
    }
}