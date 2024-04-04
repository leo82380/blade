using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    [SerializeField] private LayerMask _whatIsGround;

    public bool ScreenToWorld(Vector3 screenPos, out Vector3 worldPos)
    {
        Camera mainCam = Camera.main;
        Ray ray = mainCam.ScreenPointToRay(screenPos);
        
        bool result = Physics.Raycast(ray, out RaycastHit hit, mainCam.farClipPlane, _whatIsGround);
        
        worldPos = result ? hit.point : Vector3.zero;
        return result;
    }

    public Vector3 GetTowardMouseDirection(Transform trm, Vector3 mouseScreenPos)
    {
        bool hit = ScreenToWorld(mouseScreenPos, out Vector3 worldPos);

        if (hit)
        {
            Vector3 direction = worldPos - trm.position;
            direction.y = 0;
            return direction.normalized;
        }
        
        return trm.forward;
    }
}
