using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastVisionn : MonoBehaviour
{
    public Transform player;                  // Referencia al jugador
    [SerializeField] Transform sightOrigin;  // Punto desde donde sale el raycast
    [SerializeField] float rayDistance = 10f; // Distancia máxima de visión

    void Start()
    {
        // Si no asignaste el player, lo busca automáticamente por tag
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    // Método que usa AgentScript
    public bool CanSeePlayer()
    {
        if (player == null || sightOrigin == null)
            return false;

        Vector3 direction = player.position - sightOrigin.position;

        // Si está fuera de rango, no ve
        if (direction.magnitude > rayDistance)
            return false;

        RaycastHit hit;
        if (Physics.Raycast(sightOrigin.position, direction.normalized, out hit, rayDistance))
        {
            if (hit.transform == player)
                return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        if (sightOrigin != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(sightOrigin.position, sightOrigin.position + sightOrigin.forward * rayDistance);
        }
    }
}
