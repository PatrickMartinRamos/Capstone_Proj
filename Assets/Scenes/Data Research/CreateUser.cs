using DG.Tweening;
using TMPro;
using UnityEngine;

public class CreateUser : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private TextMeshProUGUI greetingText;
    DataLoader dataLoader;
    SaveManager saveManager;

    void Start()
    {
        dataLoader = DataLoader.instance;
        saveManager = SaveManager.Instance;
        gameObject.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InBack);
    }

    public void OnSubmitPlayerName()
    {
        string playerName = playerNameInputField.text.Trim();
        if (!string.IsNullOrEmpty(playerName))
        {
            greetingText.text = "Uploading data...";
            playerNameInputField.interactable = false;

            dataLoader.UploadData(playerName, 0, 0, 0f, (success) =>
            {
                if (success)
                {
                    greetingText.text = $"Welcome, {playerName}!";
                    DOVirtual.DelayedCall(1f, CloseCreateUserPanel); // wait a bit before closing
                }
                else
                {
                    greetingText.text = "Upload failed. Try again.";
                    playerNameInputField.interactable = true;
                }
            });
        }
        else
        {
            Debug.LogWarning("⚠️ Player Name is empty. Please enter a valid name.");
        }
    }


    public void CloseCreateUserPanel()
    {
        gameObject.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
    }

}
