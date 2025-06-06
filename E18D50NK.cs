using System.Diagnostics;
using UnityEngine;

public class E18D50NK : MonoBehaviour
{
    [Header("Settings")]
    public float maxDistance = 50f;
    public float minDistance = 2f;
    public LayerMask detectionMask;
    
    private bool isDetected;
    private float currentDistance;

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, detectionMask))
        {
            isDetected = hit.distance >= minDistance;
            currentDistance = hit.distance;
        }
        else
        {
            isDetected = false;
            currentDistance = maxDistance;
        }
    }
    public bool IsObstacleDetected()
    {
        return isDetected;
    }

    public float GetDistance()
    {
        return Mathf.Clamp(currentDistance, minDistance, maxDistance);
    }


}