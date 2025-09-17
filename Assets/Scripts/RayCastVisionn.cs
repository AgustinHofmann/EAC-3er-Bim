using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastVisionn : MonoBehaviour

{

    [SerializeField] Vector3 rayOrigin;
    [SerializeField] Transform sightOrigin;
    [SerializeField] float rayDistance;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(sightOrigin.position, sightOrigin.forward, out hitInfo, rayDistance))
        {

            Debug.Log(hitInfo.collider.gameObject);

        }
    }
    private void OnDrawGizmos()
    {
        Color color = Color.red;
        Gizmos.DrawLine(sightOrigin.position, sightOrigin.position + sightOrigin.forward * rayDistance);
    }
}