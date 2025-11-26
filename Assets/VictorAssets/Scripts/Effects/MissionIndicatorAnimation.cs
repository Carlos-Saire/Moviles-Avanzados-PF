using UnityEngine;

public class MissionIndicatorAnimation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f; 
    [SerializeField] private float floatSpeed = 1.5f;   
    [SerializeField] private float floatAmplitude = 0.2f; 

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition; 
    }

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);

        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);
    }
}