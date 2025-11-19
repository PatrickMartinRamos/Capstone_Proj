using NUnit.Framework.Internal;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingImage;

    void Start()
    {

    }

    public void test()
    {
        if (DataManager.Instance == null)
        {
            Debug.LogError("DataManager instance not found");
            return;
        }

        Debug.Log(DataManager.Instance.GetCurrentData().playerName);
    }

}
