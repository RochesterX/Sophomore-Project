using UnityEngine;
using UnityEngine.UI;

public class MapSelect : MonoBehaviour
{
    private ToggleGroup maps;

    private void Start()
    {
        maps = GetComponent<ToggleGroup>();
    }

    void Update()
    {
        Toggle toggle = maps.GetFirstActiveToggle();
        GameManager.map = toggle.name;
        print(GameManager.map);
    }
}
