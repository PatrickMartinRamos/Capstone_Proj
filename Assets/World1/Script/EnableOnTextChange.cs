using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class EnableOnTextChange : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    private string lastText;

    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        lastText = tmp.text;

        // Optional: Disable at start if text is empty
        // gameObject.SetActive(tmp.text != "");
    }

    void Update()
    {
        // If text has changed since last frame
        if (tmp.text != lastText)
        {
            lastText = tmp.text;

            // Enable this object
            if (!gameObject.activeSelf)
                gameObject.transform.parent.parent.gameObject.SetActive(true);
        }
    }
}
