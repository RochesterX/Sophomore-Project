using UnityEngine;
using Game;
using Music;
using Player;

namespace Game
{
    /// <summary>
    /// This class handles the infinite scrolling effect for the background.
    /// </summary>
    public class InfiniteScroll : MonoBehaviour
    {
        /// <summary>
        /// The speed at which the background scrolls.
        /// </summary>
        public float speed;

        /// <summary>
        /// The starting position of the background.
        /// </summary>
        public float start;

        /// <summary>
        /// The ending position of the background.
        /// </summary>
        public float end;

        /// <summary>
        /// Updates the position of the background to create a scrolling effect.
        /// </summary>
        private void Update()
        {
            // If the background moves past the end position, reset it to the start position
            if (transform.position.x > end)
            {
                transform.position = new Vector3(start, transform.position.y, transform.position.z);
            }
            // If the background moves past the start position, reset it to the end position
            else if (transform.position.x < start)
            {
                transform.position = new Vector3(end, transform.position.y, transform.position.z);
            }

            // Move the background to the right based on the speed and time elapsed
            transform.position += speed * Time.deltaTime * Vector3.right;
        }
    }
}
