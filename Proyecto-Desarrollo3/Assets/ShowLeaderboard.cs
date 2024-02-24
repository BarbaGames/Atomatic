using System.Collections;
using UnityEngine;

public class ShowLeaderboard : MonoBehaviour
{
    [SerializeField] private GameObject upPosition;
    [SerializeField] private GameObject downPosition;
    private Vector3 up;
    private Vector3 down;

    private void Start()
    {
        up = gameObject.transform.position;
        down = gameObject.transform.position;
        up.y = upPosition.transform.position.y;
        down.y = downPosition.transform.position.y;
    }

    public void MoveLeaderboardUp()
    {
        Debug.Log("move up");
        StartCoroutine(MoveUp());
    }
    public void MoveLeaderboardDown()
    {
        StartCoroutine(MoveDown());
    }
    private IEnumerator MoveUp()
    {
        Vector3 position = transform.position;
        for (float i = 0; i <= 2f; i += 0.01f)
        {
            position = new Vector3(position.x,
                Mathf.Lerp(position.y, up.y, Mathf.SmoothStep(0, 1, i)), position.y);
            transform.position = position;
        }

        yield break;
    }

    private IEnumerator MoveDown()
    {
        Vector3 position = transform.position;
        for (float i = 0; i <= 2f; i += 0.01f)
        {
            position = new Vector3(position.x,
                Mathf.Lerp(position.y, down.y, Mathf.SmoothStep(0, 1, i)), position.y);
            transform.position = position;
        }

        yield break;
    }
}