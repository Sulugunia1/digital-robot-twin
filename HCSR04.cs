using UnityEngine;

public class HCSR04 : MonoBehaviour
{
    [Header("Settings")]
    public float maxDistance = 400f;
    public float minDistance = 2f;
    public LayerMask detectionMask;
    
    private float currentDistance;

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, detectionMask))
        {
            currentDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            currentDistance = maxDistance;
        }
    }

    public float GetDistance()
    {
        return currentDistance;
    }
}