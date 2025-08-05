using System;
using CapstoneProj.ControlPanelSystem;
using CapstoneProj.CoreSystem;
using CapstoneProj.GridSystem;
using CapstoneProj.MiscSystem;
using CapstoneProj.ScriptableObjectSystem;
using UnityEngine;

namespace CapstoneProj.ProgressSystem
{
    public class Progression : SingletonBehaviour<Progression>
    {
        public event EventHandler OnProgress;

        [SerializeField] private int _points = 0;

        public void ResetProgression()
        {
            _points = 0;
            // TODO: RESET PROGRESS BAR
            // TODO: RESET PROGRESS STARS
        }

        private void Start()
        {
            DefuseButton.Instance.OnClick
                += DefuseButton_OnClick;

            ResetProgression();
        }

        private void DefuseButton_OnClick(object sender, EventArgs e)
        {
            GridNavigator gridNavigator = GridNavigator.Instance;
            Tile tile = gridNavigator.GetParentTile();

            if (tile.HasBomb())
            {
                DefuseButton.Instance.CorrectClick();

                Bomb bomb = tile.GetBomb();
                _points += bomb.GetBombSO().CorrectPoints;
                StageSO stageSO = World2GameManager.Instance.GetStageSO();
                ProgressBar.Instance.Progress(_points / (float) stageSO.RequiredPoints);

                OnProgress?.Invoke(this, EventArgs.Empty);
            }
            else
                DefuseButton.Instance.IncorrectClick();
        }
    }
}