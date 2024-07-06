using UnityEngine;

public interface IDirectMoveable
{
    public void SetMovement(Vector3 movement, bool isRotation = true);
}