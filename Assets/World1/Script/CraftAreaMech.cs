using UnityEngine;

public class CraftAreaMech : AreaContainer
{
    [SerializeField] bool isCraftArea2 = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        if (isCraftArea2)
        {
            StageManager.Instance.craftArea2 = this.gameObject;
        }
        else StageManager.Instance.craftArea = this.gameObject;
    }

}
