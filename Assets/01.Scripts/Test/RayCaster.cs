using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCaster : MonoBehaviour
{
    [SerializeField] private float _rayDistance;
    [SerializeField] private LayerMask _whatIsEnemy;

    private void OnDrawGizmos()
    {
        //bool isHit = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _rayDistance, _whatIsEnemy);

        bool isHit = Physics.BoxCast(transform.position, transform.lossyScale * 0.5f, 
            transform.forward, out RaycastHit hit, 
            transform.rotation, _rayDistance, _whatIsEnemy);
        
        if (isHit)
        {
            Gizmos.color = Color.red;
            //Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
            Gizmos.DrawWireCube(transform.position + transform.forward *
                hit.distance, transform.lossyScale);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, transform.forward * _rayDistance);
        }
    }
}
