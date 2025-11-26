using UnityEngine;

public class VisionDetector : MonoBehaviour
{
    public float viewRadius = 10f;           // distancia de visión
    public float viewAngle = 120f;           // ángulo de visión
    public LayerMask targetMask;             // quién puede ver (ej: DoppelGanger)
    public LayerMask obstacleMask;           // qué bloquea la visión

    public Transform eyePoint;               // posición desde donde mira el jugador

    private void Update()
    {
        DetectTargets();
    }

    void DetectTargets()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        foreach (var t in targets)
        {
            Transform target = t.transform;

            Vector3 dirToTarget = (target.position - eyePoint.position).normalized;

            // 1. ¿está dentro del ángulo?
            if (Vector3.Angle(eyePoint.forward, dirToTarget) < viewAngle / 2)
            {
                float dist = Vector3.Distance(eyePoint.position, target.position);

                // 2. ¿hay línea de visión?
                if (!Physics.Raycast(eyePoint.position, dirToTarget, dist, obstacleMask))
                {
                    Debug.Log("Jugador vio al Doppelganger!");
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees)
    {
        return Quaternion.Euler(0, angleInDegrees, 0) * Vector3.forward;
    }
}
