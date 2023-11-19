using UnityEngine;

public class ScientistController : MonoBehaviour
{
    public Vector3 targetPosition;
    public Vector3 targetScale;
    public float lerpSpeed = 1.0f;
    public float speed = 0.1f;

    private Vector3 initialPosition;
    private Vector3 initialScale;
    private float yScaleLerpStartTime;
    private bool isYScaleLerping = false;
    private bool isXPositionLerping = false;
    private float currentXPosition;
    private bool movingTowardsEnd = true;
    private float journeyLength;
    private float startTime;
    Vector3 pointA;
    Vector3 pointB;
   
    
    private void Start()
    {
        initialPosition = transform.localPosition;
        initialScale = transform.localScale;
    }

    private void Update()
    {
        if (isYScaleLerping)
        {
            float timeSinceStart = Time.time - yScaleLerpStartTime;
            float percentageComplete = timeSinceStart / lerpSpeed;

            // Lerp Y position and scale
            transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, percentageComplete);
            transform.localScale = Vector3.Lerp(initialScale, targetScale, percentageComplete);

            if (percentageComplete >= 1.0f)
            {
                isYScaleLerping = false;
                
                StartXPositionLerp();
            }
        }

        if (isXPositionLerping)
        {
            //PingPong between 0 and 1
            float time = Mathf.PingPong((Time.time - startTime) * speed, 1);
            transform.localPosition = Vector3.Lerp(pointA, pointB, time);

            if (transform.localPosition.x >= pointB.x - 1)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (transform.localPosition.x <= pointA.x + 1)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public void StartYScaleLerp()
    {
        yScaleLerpStartTime = Time.time;
        isYScaleLerping = true;
    }

    private void StartXPositionLerp()
    {
        speed = Random.Range(0.1f, 0.5f);
        pointA = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        pointB = new Vector3(419, transform.localPosition.y, transform.localPosition.z);
        isXPositionLerping = true;
        startTime = Time.time;
    }
}
