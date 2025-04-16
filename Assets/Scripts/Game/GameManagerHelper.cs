using UnityEngine; using Game; using Music; using Player;
namespace Game
{

[ExecuteAlways]
public class GameManagerHelper : MonoBehaviour
{
    public bool addHatPosition;
    public bool addSpawnPosition;


    private void Update()
    {
        if (addHatPosition)
        {
            addHatPosition = false;
            GetComponent<GameManager>().hatSpawnPositions.Add(GameObject.Find("HELPER").transform.position);
        }
        if (addSpawnPosition)
        {
            addSpawnPosition = false;
            GetComponent<GameManager>().spawnPosition = GameObject.Find("HELPER").transform.position;
        }
    }
}
}