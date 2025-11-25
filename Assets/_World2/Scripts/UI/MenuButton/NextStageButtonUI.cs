using UnityEngine;

namespace Stellarfarer
{
    public class NextStageButtonUI : MenuButtonUI
    {
        private const string STAGE_ID_NAME = "StageID";

        protected override void ButtonAction()
        {
            int stageID = Hydros7WorldManager.Instance.GetStageID();
            stageID = PlayerPrefs.HasKey(STAGE_ID_NAME) ? stageID + 1 : 1;
            PlayerPrefs.SetInt(STAGE_ID_NAME, stageID);
            LoadWorld2();
        }
    }
}