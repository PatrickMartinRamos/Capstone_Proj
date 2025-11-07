using DG.Tweening;
using TMPro;
using UnityEngine;

public class CreateUser : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private TextMeshProUGUI greetingText;
    [SerializeField] private GameObject playBtn;
    DataLoader dataLoader;
    //SaveManager saveManager;

    void Start()
    {
        dataLoader = DataLoader.instance;
        //saveManager = SaveManager.Instance;
        gameObject.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InBack);
        if (dataLoader == null)
        {
            Debug.LogWarning("⚠️ DataLoader instance not found in CreateUser.");
            return;
        }

        string currentUser = dataLoader.GetCurrentUser();

        // If no user is present, leave the panel open so user can create one.
        if (string.IsNullOrEmpty(currentUser) || currentUser == "No User")
        {
            Debug.Log("There is no user");
            playBtn.SetActive(false);
            return;
        }

        // User exists — show greeting and close the create-user panel.
        Debug.Log($"User found: {currentUser}");
        if (greetingText != null)
            greetingText.text = $"Welcome back, {currentUser}!";
            playBtn.SetActive(true);
        if (playerNameInputField != null)
            playerNameInputField.interactable = false;

        // Close with the same animation used elsewhere.
        DOVirtual.DelayedCall(0.1f, CloseCreateUserPanel);
    }


    public void OnSubmitPlayerName()
    {
        string playerName = playerNameInputField.text.Trim();
        if (!string.IsNullOrEmpty(playerName))
        {
            greetingText.text = "Loading...";
            playerNameInputField.interactable = false;

            dataLoader.UploadData(playerName, 0, 0, 0f, (success) =>
            {
                if (success)
                {
                    greetingText.text = $"Welcome, {playerName}!";
                    playBtn.SetActive(true);
                    DOVirtual.DelayedCall(1f, CloseCreateUserPanel); // wait a bit before closing
                }
                else
                {
                    greetingText.text = "Upload failed. Try again.";
                    playBtn.SetActive(false);
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
