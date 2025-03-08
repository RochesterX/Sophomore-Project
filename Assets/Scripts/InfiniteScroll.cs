using UnityEngine;

public class InfiniteScroll : MonoBehaviour
{
    public float speed;
    public float start;
    public float end;

    private void Update()
    {
        if (transform.position.x > end)
        {
            transform.position = new Vector3(start, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < start)
        {
            transform.position = new Vector3(end, transform.position.y, transform.position.z);
        }

        transform.position += speed * Time.deltaTime * Vector3.right;
    }
}
