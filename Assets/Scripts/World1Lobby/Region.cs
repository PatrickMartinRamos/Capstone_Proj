using System.Collections;
using UnityEngine;

public class Region : MonoBehaviour
{
    [SerializeField] private int stageNumber;
    [SerializeField] private bool isUnlocked = false, isSelected = false;
    //[SerializeField] private GameObject stageLock;
    [SerializeField][Range(0, 3)] private float clearLevel;

    [SerializeField] private Vector3 targetEulerAngle;
    private Vector3 currentEulerAngle;
    [SerializeField] private float rotateTime = 2f, time;

    private bool isWorldRotating = false;
    private GameObject World;

    private void Update()
    {
        if (isSelected && isWorldRotating)
        {
            StartCoroutine(RotateOverTime());
            if(World.transform.eulerAngles == targetEulerAngle)
            {
                StopCoroutine(RotateOverTime());
                isWorldRotating = false;
            }
        }
    }
    public void UnselectRegion()
    {
        isSelected = false;
    }
    public void RotateWorld(GameObject world)
    {
        isSelected = true;
        World = world;
        currentEulerAngle = World.transform.eulerAngles;
        isWorldRotating = true;
    }
    IEnumerator RotateOverTime()
    {
        Vector3 startEuler = World.transform.eulerAngles;
        float time = 0f;

        while (time < rotateTime)
        {
            time += Time.deltaTime;
            float t = time / rotateTime;
            World.transform.eulerAngles = Vector3.Lerp(startEuler, targetEulerAngle, t);
            yield return null;
        }

        // Ensure final angle is set
        World.transform.eulerAngles = targetEulerAngle;
    }
}
