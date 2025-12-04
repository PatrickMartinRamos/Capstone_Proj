using UnityEngine;

namespace Stellarfarer
{
    public class NextStageButtonUI : MenuButtonUI
    {
        private const string STAGE_ID_NAME = "StageID";

        protected override void Awake()
        {
            base.Awake();

            Show();
        }

        protected override void ButtonAction()
        {
            int stageID = Hydros7WorldManager.Instance.GetStageID();

            if (stageID < Hydros7WorldManager.Instance.GetStageCount())
            {
                stageID++;
                PlayerPrefs.SetInt(STAGE_ID_NAME, stageID);
                LoadWorld2();
            }
            else
                Hide();
        }
    }
}