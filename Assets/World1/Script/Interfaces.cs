using UnityEngine;

public interface IDraggable
{
    bool Interact(GameObject target, Vector3 spawnPt);
    ShapeClassification GetDraggedObjectClassification();
}
public interface Itargetable
{
    ShapeStats SendInfo();
    ShapeClassification GetTargetClassification();
}