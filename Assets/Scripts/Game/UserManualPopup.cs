using UnityEngine;
using TMPro;

/// <summary>
/// This class manages the user manual popup, including displaying instructions,
/// game mode descriptions, controls, and tips for players.
/// </summary>
public class UserManualPopup : MonoBehaviour
{
    /// <summary>
    /// The panel that contains the user manual popup.
    /// </summary>
    public GameObject popupPanel;

    /// <summary>
    /// The text element used to display the user manual content.
    /// </summary>
    public TextMeshProUGUI userManualText;

    /// <summary>
    /// The sprite asset used for embedding icons in the user manual text.
    /// </summary>
    public TMP_SpriteAsset combinedSpriteAsset;

    /// <summary>
    /// Initializes the user manual content and assigns the sprite asset.
    /// </summary>
    private void Start()
    {
        // Assign the combined sprite asset
        userManualText.spriteAsset = combinedSpriteAsset;

        // Clear the text
        userManualText.text = "";

        // Add the header
        userManualText.text += "<align=center><size=60><b>User Manual � How to Play</b></size></align>\n\n";

        // Add the first section
        userManualText.text += "<size=30>Press the 'Start' Button to enter the main menu.</size>\n";
        userManualText.text += "<size=30>Use <sprite=1> on keyboard, or <sprite=13> on controller to add your player.</size>\n";
        userManualText.text += "<size=30>Choose your desired gamemode and map.</size>\n";
        userManualText.text += "<size=30>Press 'Begin' to start the match.</size>\n\n";

        // Add the "Game Modes" section
        userManualText.text += "<align=center><size=50><b>Game Modes</b></size></align>\n\n";

        userManualText.text += "<size=40><b>Free For All</b></size>\n";
        userManualText.text += "<size=30>Battle it out with other players.</size>\n";
        userManualText.text += "<size=30>Each player starts with 5 lives.</size>\n";
        userManualText.text += "<size=30>The last player standing wins.</size>\n\n";

        userManualText.text += "<size=40><b>Obstacle Course</b></size>\n";
        userManualText.text += "<size=30>Punch and race your way through the course to reach the end.</size>\n";
        userManualText.text += "<size=30>The player who reaches the flag first wins.</size>\n\n";

        userManualText.text += "<size=40><b>Keep Away</b></size>\n";
        userManualText.text += "<size=30>Fight for the hat! You get 3 minutes.</size>\n";
        userManualText.text += "<size=30>Touch the hat to start wearing it.</size>\n";
        userManualText.text += "<size=30>You will drop the hat if you are hit or if you died.</size>\n";
        userManualText.text += "<size=30>The player who wears the hat the longest wins.</size>\n\n";

        // Add the "Controls" section
        userManualText.text += "<align=center><size=50><b>Controls</b></size></align>\n\n";
        userManualText.text += "<size=40><b>Naviagation:</b></size><size=30> Use Left-Click to select and press <sprite=17> to exit</size>\n";
        userManualText.text += "<size=40><b>Move:</b></size><size=30> <sprite=16> / <sprite=7><sprite=4><sprite=6><sprite=0></size>\n";
        userManualText.text += "<size=40><b>Jump:</b></size><size=30> <sprite=8> / <sprite=10> / <sprite=14> / <sprite=5></size>\n";
        userManualText.text += "<size=40><b>Punch:</b></size><size=30> <sprite=11> / <sprite=9> / <sprite=15> / <sprite=2></size>\n";
        userManualText.text += "<size=40><b>Block:</b></size><size=30> <sprite=12></size>\n";
        userManualText.text += "<size=40><b>Parry:</b></size><size=30> Let go of <sprite=12> at the right time</size>\n\n";

        // Add the "Tips" section
        userManualText.text += "<align=center><size=50><b>Tips</b></size></align>\n\n";
        userManualText.text += "<size=30>Punches seem to hit you harder when you lose health.</size>\n\n";
        userManualText.text += "<size=30>Walk into your opponent to turn them around and push them.</size>\n\n";
        userManualText.text += "<size=30>Time your jumps to avoid falling (or hitting your head).</size>\n\n";
        userManualText.text += "<size=30>The hat has physics too! Don't punch it too hard.</size>";
    }

    /// <summary>
    /// Displays the user manual popup.
    /// </summary>
    public void ShowPopup()
    {
        popupPanel.SetActive(true);
        gameObject.SetActive(true);
        print("User manual opened");
    }

    /// <summary>
    /// Hides the user manual popup.
    /// </summary>
    public void HidePopup()
    {
        popupPanel.SetActive(false);
        gameObject.SetActive(false);
        print("User manual closed");
    }
}