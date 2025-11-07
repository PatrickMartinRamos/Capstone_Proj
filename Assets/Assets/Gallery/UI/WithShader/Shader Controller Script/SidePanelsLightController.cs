using UnityEngine;
using UnityEngine.UI;

public class SidePanelsLightController : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;
    private Material _mat;
    [SerializeField] private float glow;
    [SerializeField] private float glowSpeed;

    void Start()
    {   
        _mat = Instantiate(rawImage.material);
        rawImage.material = _mat;
        // Debug.Log(_mat.SetFloat("GlowStrength", 2f));
        // Debug.Log(_mat.HasProperty("_GlowStrength"));
    }

    void Update()
    {
        float animatedGlow = Mathf.PingPong(Time.time * glowSpeed, glow);
        _mat.SetFloat("_GlowStrength", animatedGlow);
    }
}
