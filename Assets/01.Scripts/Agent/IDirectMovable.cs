using UnityEngine;

public interface IDirectMovable
{
    public void SetMovement(Vector3 movement, bool isRotation = true);
}
