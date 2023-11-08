using UnityEngine;

public class ScientistController : MonoBehaviour
{
    public Vector3 targetPosition;
    public Vector3 targetScale;
    public Vector2 xPositionRange;
    public float lerpSpeed = 1.0f;
    public float lerpSpeedX = 10.0f;

    private Vector3 initialPosition;
    private Vector3 initialScale;
    private float yScaleLerpStartTime;
    private bool isYScaleLerping = false;
    private bool isXPositionLerping = false;
    private float currentXPosition;

    
    public Vector3 startPoint;
    public Vector3 endPoint;
    public float speed = 2.0f;

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
            float time = Mathf.PingPong(Time.time * speed, 1);
            transform.localPosition = Vector3.Lerp(pointA, pointB, time);
        }
    }

    public void StartYScaleLerp()
    {
        yScaleLerpStartTime = Time.time;
        isYScaleLerping = true;
    }

    private void StartXPositionLerp()
    {
        pointA = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        pointB = new Vector3(419, transform.localPosition.y, transform.localPosition.z);
        isXPositionLerping = true;
    }
}
