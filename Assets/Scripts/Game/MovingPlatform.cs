using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform platform;
    public int startPoint;
    public Transform[] points;
    public float speed;
    private int i;

    void Start() // Sets the initial position of the platform
    {
        transform.position = points[startPoint].position;
    }

    void Update()
    {
        // If the platform is close to the target point, it starts moving to the next one
        if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
        }
        // Moves the platform towards the next point
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }
}
