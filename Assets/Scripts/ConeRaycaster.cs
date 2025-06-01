using System.Collections.Generic;
using UnityEngine;

public class ConeRaycaster : MonoBehaviour
{
    [Header("Cone Settings")]
    public float coneAngle = 45f;
    public float coneRange = 10f;
    public int rayCount = 30;

    [Header("Detection Settings")]
    public LayerMask detectionLayer;
    public string enemyTag = "Enemy";

    [Header("Debug")]
    public bool showGizmos = true;

    private readonly List<Transform> detectedEnemies = new List<Transform>();
    public List<Transform> DetectedEnemies => detectedEnemies;
    [SerializeField] VisionCone _visionCone;
    public void ResetCone()
    {
        if (_visionCone == null)
            _visionCone = GetComponentInChildren<VisionCone>();
        _visionCone.VisionAngle = coneAngle;
        _visionCone.VisionRange = coneRange;
        _visionCone.ResetCone();
    }
    public void PerformConeRaycast()
    {
        detectedEnemies.Clear();

        float halfAngle = coneAngle / 2f;
        Vector3 forward = transform.forward;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = Mathf.Lerp(-halfAngle, halfAngle, i / (rayCount - 1f));
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 direction = rotation * forward;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, coneRange, detectionLayer))
            {
                if (hit.collider.CompareTag(enemyTag) && !detectedEnemies.Contains(hit.collider.transform))
                {
                    detectedEnemies.Add(hit.rigidbody.transform);
                    Debug.DrawLine(transform.position, hit.point, Color.red);
                }
                else
                {
                    Debug.DrawLine(transform.position, transform.position + direction * coneRange, Color.yellow);
                }
            }
            else
            {
                Debug.DrawLine(transform.position, transform.position + direction * coneRange, Color.green);
            }
        }
        detectedEnemies.Sort((a, b) => (transform.position- a.position).sqrMagnitude.CompareTo((transform.position - b.position).sqrMagnitude));
    }
    private void Update()
    {
        //_visionConeTransform.position = transform.position;
    }
    void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;

        float halfAngle = coneAngle / 2f;
        Vector3 leftDir = Quaternion.Euler(0, -halfAngle, 0) * transform.forward;
        Vector3 rightDir = Quaternion.Euler(0, halfAngle, 0) * transform.forward;

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, leftDir * coneRange);
        Gizmos.DrawRay(transform.position, rightDir * coneRange);
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }

    public List<Transform> GetDetectedEnemies()
    {
        return detectedEnemies;
    }
}
