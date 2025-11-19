using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingImage;

    void Start()
    {
        if (DataManager.Instance == null)
        {
            Debug.LogError("DataManager instance not found");
            return;
        }

        // Wait until cloud data is loaded
        DataManager.Instance.SyncCloudData(null, loadingImage, cloudData =>
        {
            if (cloudData == null)
            {
                Debug.LogError("Failed to load player data");
                return;
            }

            string playerName = cloudData.playerName;
            Debug.Log("Player name is: " + playerName);

            // Do anything else that depends on playerName here
        });
    }

}
