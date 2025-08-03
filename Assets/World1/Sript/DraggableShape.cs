using UnityEngine;

public class DraggableShape : MonoBehaviour
{
    [SerializeField]
    private Vector3 originPosition;
    [SerializeField]
    private string name;
    public string Name => name;
    public void Start()
    {
        originPosition = transform.position;
    }
    public void RevertPosition()
    {
        this.transform.position = originPosition;
    }
}
