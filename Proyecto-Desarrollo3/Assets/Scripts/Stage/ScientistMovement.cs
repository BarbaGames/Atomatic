using UnityEngine;

namespace Stage
{
    public class ScientistMovement : MonoBehaviour
    {
        [SerializeField] private float scientistSpeed = 1;
        [SerializeField] private Transform start;
        [SerializeField] private Transform finish;
        private int _direction = 1;

        private void Update()
        {
            transform.position += Vector3.right * (_direction * (scientistSpeed * Time.deltaTime));
            TurnAround();
        }

        private void TurnAround()
        {
            if (transform.position.x > finish.position.x)
            {
                _direction *= -1;
                transform.Rotate(0, 180, 0);
            }

            if (transform.position.x <start.position.x)
            {
                _direction *= -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}