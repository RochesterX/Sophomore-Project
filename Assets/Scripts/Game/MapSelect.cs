using UnityEngine; using Game; using Music; using Player;
using UnityEngine.UI;
namespace Game
{

public class MapSelect : MonoBehaviour
{
    private ToggleGroup maps;

    private void Start()
    {
        maps = GetComponent<ToggleGroup>();
    }

    void Update() // Sets the map based on the selected toggle
    {
        Toggle toggle = maps.GetFirstActiveToggle();
        GameManager.map = toggle.name;
    }
}
}