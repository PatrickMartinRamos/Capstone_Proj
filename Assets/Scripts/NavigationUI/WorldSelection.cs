using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldSelection : MonoBehaviour
{
    [SerializeField] private string worldName, sceneName;
    [SerializeField] private bool isUnlocked, isSelected, isLastSelected = false;

    private void Update()
    {
        transform.Rotate(0, 1, 0);
    }

    public void EnterWorld()
    {
        SceneManager.LoadScene(sceneName);
    }
    public string Selected()
    {
        return worldName;
    }
}
