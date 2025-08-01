using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GeneralUIManagerScript : MonoBehaviour
{
    [Header("Labels")] 
    [SerializeField] private TextMeshProUGUI volumeTxt;
    [Header("Buttons")]
    [SerializeField] private GameObject BackButton;
    [SerializeField] private GameObject SettingsButton, SettingsHomeButton, SettingsRestartButton, SettingsBackButton;
    [Header("Sliders")]
    [SerializeField] private GameObject VolumeSlider;
    [Header("Panel")]
    [SerializeField] private GameObject SettingsPanel;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BackButton.GetComponent<Button>().onClick.AddListener(Back);
        SettingsButton.GetComponent<Button>().onClick.AddListener(Settings);
        SettingsHomeButton.GetComponent<Button>().onClick.AddListener(Home);
        SettingsRestartButton.GetComponent<Button>().onClick.AddListener(RestartScene);
        SettingsBackButton.GetComponent<Button>().onClick.AddListener(SettingsBack);
        VolumeSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { UpdateVolumeTxt(); });
    }
    void UpdateVolumeTxt()
    {
        volumeTxt.text = VolumeSlider.GetComponent<Slider>().value.ToString();
    }
    void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    void Home()
    {
        SceneManager.LoadScene("MainLobby");
    }
    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
