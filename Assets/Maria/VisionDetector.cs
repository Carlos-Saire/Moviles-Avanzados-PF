using System;
using UnityEngine;

public class VisionDetector : MonoBehaviour
{
    public float viewRadius = 10f;
    public float viewAngle = 120f;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public Transform eyePoint;
    public event Action<bool> OnDoppleWatched;
    bool hasWatched = false;

    public GameObject back;

    private void Update()
    {
        DetectTargets();
    }

    void DetectTargets()
    {
        Collider[] targets = Physics.OverlapSphere(eyePoint.position, viewRadius, targetMask);

        bool seeing = false;

        foreach (var t in targets)
        {
            Transform target = t.transform;
            Vector3 dirToTarget = (target.position - eyePoint.position).normalized;

            // 1 → Dentro del ángulo
            if (Vector3.Angle(eyePoint.forward, dirToTarget) < viewAngle / 2)
            {
                float dist = Vector3.Distance(eyePoint.position, target.position);

                // 2 → Sin obstáculos
                if (!Physics.Raycast(eyePoint.position, dirToTarget, dist, obstacleMask))
                {
                    seeing = true;
                    //vio al doppleganger
                    OnDoppleWatched?.Invoke(hasWatched);
                    Debug.Log("Dopple watched state changed: " + hasWatched);
                    break;
                }
            }
        }

        if (seeing != hasWatched)
        {
            hasWatched = seeing;
            OnDoppleWatched?.Invoke(hasWatched);
            Debug.Log("Dopple watched state changed: " + hasWatched);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees)
    {
        return Quaternion.Euler(0, angleInDegrees, 0) * Vector3.forward;
    }

    // ⭐⭐⭐ VISUALIZACIÓN DEL CONO DE VISIÓN ⭐⭐⭐
    private void OnDrawGizmos()
    {
        if (eyePoint == null) return;

        Gizmos.color = new Color(1, 1, 0, 0.25f); // amarillo transparente
        Gizmos.DrawWireSphere(eyePoint.position, viewRadius);

        // líneas del ángulo de visión
        Vector3 leftBoundary = DirFromAngle(-viewAngle / 2);
        Vector3 rightBoundary = DirFromAngle(viewAngle / 2);

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(
            eyePoint.position,
            eyePoint.position + leftBoundary * viewRadius
        );

        Gizmos.DrawLine(
            eyePoint.position,
            eyePoint.position + rightBoundary * viewRadius
        );

        // Rellenar cono (opcional pero bonito y ligero)
        Gizmos.color = new Color(1, 1, 0, 0.1f);

        int segments = 40;
        float deltaAngle = viewAngle / segments;

        for (int i = 0; i < segments; i++)
        {
            float a = -viewAngle / 2 + deltaAngle * i;
            float b = -viewAngle / 2 + deltaAngle * (i + 1);

            Vector3 p1 = eyePoint.position + DirFromAngle(a) * viewRadius;
            Vector3 p2 = eyePoint.position + DirFromAngle(b) * viewRadius;

            Gizmos.DrawLine(p1, p2);
        }
    }
}
