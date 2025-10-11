using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldSelection : MonoBehaviour
{
    [SerializeField] private string worldName, sceneName;
    [SerializeField] private float worldRotationSpeed;
    [SerializeField] private bool isUnlocked, isSelected = false;

    private void Update()
    {
        float speed = worldRotationSpeed * Time.deltaTime;
        transform.Rotate(0, 1 * speed, 0);
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
