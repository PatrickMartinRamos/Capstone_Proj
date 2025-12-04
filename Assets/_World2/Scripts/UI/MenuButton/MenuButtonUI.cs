using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Stellarfarer
{
    [RequireComponent(typeof(Button))]
    public class MenuButtonUI : MonoBehaviour
    {
        private const string WORLD2SCENE_FILEPATH = "Scenes/Game Scene/World2_GameScene";
        private const string WORLDSELECTION_FILEPATH = "Scenes/WorldSelection";

        [SerializeField] protected Button _button;

        protected virtual void Awake()
        {
            if (_button == null)
                _button = GetComponent<Button>();

            _button.onClick.AddListener(ButtonAction);
        }

        protected virtual void ButtonAction() { }

        protected void LoadWorld2()
            => LoadScene(WORLD2SCENE_FILEPATH);

        protected void LoadWorldSelection()
            => LoadScene(WORLDSELECTION_FILEPATH);

        private void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
            Time.timeScale = 1f;
        }

        public void Show()
            => SetVisibility(true);

        public void Hide()
            => SetVisibility(false);

        private void SetVisibility(bool isVisible)
            => gameObject.SetActive(isVisible);
    }
}