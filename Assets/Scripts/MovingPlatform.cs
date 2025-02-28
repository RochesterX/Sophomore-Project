using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform platform;
    public int startPoint;
    public Transform[] points;
    public float speed;

    private int i;

    void Start()
    {
        transform.position = points[startPoint].position;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }
}
