using Unity.VisualScripting;
using UnityEngine;

public class shapeCombination : MonoBehaviour
{
    public static shapeCombination Instance {  get; private set; }
    [SerializeField] private Sprite triangle;
    //[SerializeField] private Sprite triangle;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public Sprite TriangleCombination()
    {
        return triangle;
    }

    //public Sprite TriangleCombination()
    //{
    //    return 
    //}
}
