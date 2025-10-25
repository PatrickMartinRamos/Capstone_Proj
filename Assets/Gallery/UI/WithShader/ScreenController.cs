using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public static ScreenController Instance;

    [SerializeField] private List<TextMeshProUGUI> TextMeshProUGUIs;
    [SerializeField] private TMP_FontAsset _fontAssetWithShader;
    [SerializeField] private TMP_FontAsset _defaultFontAsset;
    [SerializeField] private float duration = 0.5f;

    private float timer = 0f;
    private bool isChanged = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {

        if (isChanged)
        {
            timer += Time.deltaTime;
            if (timer >= duration)
            {
                RevertFont();
            }
        }
    }

    public void OnClickChangeFont()
    {
        ChangeFont(_fontAssetWithShader);
        isChanged = true;
        timer = 0f; // reset timer
    }

    private void ChangeFont(TMP_FontAsset newFont)
    {
        foreach (var tmp in TextMeshProUGUIs)
        {
            if (tmp != null)
            {
                tmp.font = newFont;
                tmp.fontMaterial = newFont.material; // optional: applies font's material/shader
            }
        }
    }

    private void RevertFont()
    {
        ChangeFont(_defaultFontAsset);
        isChanged = false;
        timer = 0f;
    }
}
