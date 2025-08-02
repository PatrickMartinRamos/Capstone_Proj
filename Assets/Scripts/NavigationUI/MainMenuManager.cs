using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Labels")]
    [SerializeField] private TextMeshProUGUI volumeTxt;
    [Header("Buttons")]
    [SerializeField] private GameObject PlayButton;
    [SerializeField] private GameObject SettingsButton, QuitButton, SettingsBackButton;
    [Header("Sliders")]
    [SerializeField] private GameObject VolumeSlider;
    [Header("Panel")]
    [SerializeField] private GameObject SettingsPanel;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SettingsButton.GetComponent<Button>().onClick.AddListener(Settings);
        PlayButton.GetComponent<Button>().onClick.AddListener(StartGame);
        QuitButton.GetComponent<Button>().onClick.AddListener(QuitGame);
        SettingsBackButton.GetComponent<Button>().onClick.AddListener(SettingsBack);
        VolumeSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { UpdateVolumeTxt(); });
        SetupScreen();
    }
    void SetupScreen()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }
    void UpdateVolumeTxt()
    {
        volumeTxt.text = VolumeSlider.GetComponent<Slider>().value.ToString();
    }
    void StartGame()
    {
        SceneManager.LoadScene("WorldSelection");
    }
    void QuitGame()
    {
        Application.Quit();
    }
    void Settings()
    {
        SettingsPanel.SetActive(true);
        SettingsButton.GetComponent<Button>().interactable = false;
    }
    void SettingsBack()
    {
        SettingsPanel.SetActive(false);
        SettingsButton.GetComponent<Button>().interactable = true;
    }
}

