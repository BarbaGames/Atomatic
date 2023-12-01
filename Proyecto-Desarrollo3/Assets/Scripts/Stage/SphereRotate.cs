using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 10f;

    void Update()
    {
        // Rotate the GameObject around its Y-axis
        transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
    }
}