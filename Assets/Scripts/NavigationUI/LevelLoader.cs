using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    [SerializeField] private GameObject transitionAnimator;
    private Animator animator;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        animator = transitionAnimator.GetComponentInChildren<Animator>();
    }

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(PlayTransition(sceneName));
    }

    private IEnumerator PlayTransition(string sceneName)
    {
        // Play the transition animation
        animator.SetTrigger("startTransition");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}
