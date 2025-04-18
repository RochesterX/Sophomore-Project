using UnityEngine;
using Game;
using Music;
using Player;

namespace Game
{
    /// <summary>
    /// This class controls a platform that moves between specified points in a loop.
    /// </summary>
    public class MovingPlatform : MonoBehaviour
    {
        /// <summary>
        /// The platform object that will move.
        /// </summary>
        public Transform platform;

        /// <summary>
        /// The index of the starting point for the platform.
        /// </summary>
        public int startPoint;

        /// <summary>
        /// An array of points that the platform will move between.
        /// </summary>
        public Transform[] points;

        /// <summary>
        /// The speed at which the platform moves.
        /// </summary>
        public float speed;

        /// <summary>
        /// The current target point index.
        /// </summary>
        private int i;

        /// <summary>
        /// Sets the initial position of the platform to the starting point.
        /// </summary>
        private void Start()
        {
            transform.position = points[startPoint].position;
        }

        /// <summary>
        /// Moves the platform between points in a loop.
        /// </summary>
        private void FixedUpdate()
        {
            // If the platform is close to the target point, update to the next point
            if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
            {
                i++;
                if (i == points.Length)
                {
                    i = 0; // Loop back to the first point
                }
            }

            // Move the platform towards the current target point
            GetComponent<Rigidbody2D>().MovePosition(
                Vector2.MoveTowards(transform.position, points[i].position, speed * Time.fixedDeltaTime)
            );
        }
    }
}