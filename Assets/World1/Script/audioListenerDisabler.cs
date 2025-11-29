using System.Xml.Schema;
using UnityEngine;

public class audioListenerDisabler : MonoBehaviour
{
    private AudioListener audioListener;
    int activeListenerCount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioListener = GetComponent<AudioListener>();
        activeListenerCount = CountEnabledAudioListeners();
        if (audioListener.isActiveAndEnabled && activeListenerCount > 1)
        {
            Destroy(audioListener);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private int CountEnabledAudioListeners()
    {
        var listeners = Object.FindObjectsByType<AudioListener>(
            FindObjectsInactive.Include, FindObjectsSortMode.None);

        int enabledCount = 0;
        foreach (var l in listeners)
        {
            if (l != null && l.enabled && l.gameObject.activeInHierarchy)
                enabledCount++;
        }

        return enabledCount;
    }
}
