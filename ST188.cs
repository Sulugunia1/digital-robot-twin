using UnityEngine;

public class ST188 : MonoBehaviour
{
    [Header("Settings")]
    public float maxDetect = 0.013f;
    public float minDetect = 0.004f;
    public LayerMask lineMask;
    
    private bool isLDetect;

    void Update()
    {
        RaycastHit hit;
        isLDetect = Physics.Raycast(transform.position, -transform.up, out hit, maxDetect, lineMask) && 
                       hit.distance >= minDetect;
    }

    public bool IsLineDetected()
    {
        return isLDetect;
    }
}