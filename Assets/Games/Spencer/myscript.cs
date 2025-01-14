using UnityEngine;

public class myscript : MonoBehaviour
{
    [SerializeField] private float speed;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
    }
}
