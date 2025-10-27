using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// transition animator for loading levels
/// </summary>
public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    [SerializeField] private GameObject transitionAnimator;
    private Animator animator;
    bool isTransitioning = false;

    // Flag to skip transitionIn on the first scene load
    private bool sceneLoaded = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        animator = transitionAnimator.GetComponentInChildren<Animator>();
        // Subscribe after animator is assigned
        SceneManager.sceneLoaded += OnSceneLoaded;
        // flag that the first scene is now running
        sceneLoaded = true;
    }

    public void LoadLevel(string sceneName)
    {
        if (isTransitioning) return;

        StartCoroutine(PlayTransition(sceneName));
    }

    private IEnumerator PlayTransition(string sceneName)
    {
        isTransitioning = true;

        animator.SetTrigger("startTransition");
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
        yield return null;
        isTransitioning = false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (sceneLoaded)
        {
            animator.SetTrigger("transitionIn");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
