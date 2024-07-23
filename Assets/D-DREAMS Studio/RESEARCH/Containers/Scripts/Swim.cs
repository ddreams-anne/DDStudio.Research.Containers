using UnityEngine;

namespace DDStudio.Research.Containers
{
    public class Swim : MonoBehaviour
    {
        [Tooltip("The speed at which a GameObject will move.")]
        [SerializeField]
        private float _swimSpeed = 1.0f;

        [Tooltip("The speed at which a GameObject will turn.")]
        [SerializeField]
        private float _rotationSpeed = 1.0f;

        // Direction to swim towards
        private Vector3 _swimDirection = Vector3.forward;

        // Rotation based on the target direction
        private Quaternion _swimRotation;

        // The time between direction changes
        private float _directionChangeInterval = 3.0f;

        // Time at which the direction will change
        private float _timeUntilDirectionChange;

        private void Update()
        {
            transform.Translate(_swimSpeed * Time.deltaTime * Vector3.forward);

            // Smoothly rotate the fish towards the target direction
            transform.rotation = Quaternion.Slerp(transform.rotation, _swimRotation, _rotationSpeed * Time.deltaTime);

            if (Time.time >= _timeUntilDirectionChange) ChangeDirection();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Dummy DumDum has entered the container");
        }

        private void OnTriggerStay(Collider other)
        {
            Debug.Log("Dummy DumDum is staying inside the container");
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("Dummy DumDum has left the container");
        }

        private void ChangeDirection()
        {
            // Calculate a random new direction
            float randomX = Random.Range(-1.0f, 1.0f);
            float randomY = Random.Range(-1.0f, 1.0f);
            float randomZ = Random.Range(-1.0f, 1.0f);

            _swimDirection = new Vector3(randomX, randomY, randomZ).normalized;

            // Set the next time to change direction
            _timeUntilDirectionChange = Time.time + _directionChangeInterval;

            // Calculate the rotation based on the new direction
            _swimRotation = Quaternion.LookRotation(_swimDirection);
        }
    }
}
