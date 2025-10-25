using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace StageLoader
{
    [RequireComponent(typeof(Button))]
    public class StageLoader : MonoBehaviour
    {
        private enum World
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

            _stageSelectButton.onClick.AddListener(() =>
            {
                string stageIDName = "StageID";
                PlayerPrefs.SetInt(stageIDName, _stageID);
                SceneManager.LoadScene(_world.ToString());
            });
            
            var _stageLabelTxt = GetComponentInChildren<TextMeshProUGUI>();
            _stageLabelTxt.text = _stageID.ToString();
        }

    }
} 