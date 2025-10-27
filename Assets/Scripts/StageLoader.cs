using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StageLoader
{
    [RequireComponent(typeof(Button))]
    public class StageLoader : MonoBehaviour
    {
        public enum World
        {
            World1_GameScene,
            World2_GameScene
        }

        [SerializeField] private World _world;
        [SerializeField, Range(1, 15)] private int _stageID;
        [SerializeField] private Button _stageSelectButton;

        private void Awake()
        {
            if (_stageSelectButton == null)
                _stageSelectButton = GetComponent<Button>();

            _stageSelectButton.onClick.AddListener(OnStageSelected);

            var _stageLabelTxt = GetComponentInChildren<TextMeshProUGUI>();
            _stageLabelTxt.text = _stageID.ToString();
        }

        private void OnStageSelected()
        {
            PlayerPrefs.SetInt("StageID", _stageID);
            PlayerPrefs.SetString("WorldName", _world.ToString());

            // Instead of changing scenes, tell the ComicManager to play
            ComicManager comicManager = FindAnyObjectByType<ComicManager>();
            if (comicManager != null)
            {
                comicManager.PlayComicFor(_world.ToString(), _stageID);
            }
            else
            {
                Debug.LogWarning("ComicManager not found in scene!");
            }
        }
    }
}
