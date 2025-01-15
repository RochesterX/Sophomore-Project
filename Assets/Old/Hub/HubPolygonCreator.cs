using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class HubPolygonCreator : MonoBehaviour
{
    public List<GameObject> vertices;
    public float size = 8;

    private void Update()
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i].transform.localPosition = new Vector3(size * Mathf.Cos(i * (2 * Mathf.PI / vertices.Count)), size * Mathf.Sin(i * (2 * Mathf.PI / vertices.Count)), 0);
        }
    }
}
