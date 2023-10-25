using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScoreUIController : MonoBehaviour
{
    private int PlayerIndex;
    [SerializeField]
    private TextMeshProUGUI playerScoreText;
    // Start is called before the first frame update
    public void setPlayerIndex(int pi)
    {
        PlayerIndex = pi;
        playerScoreText.SetText("Player " + (pi + 1).ToString() + " score: 0");
    }

    // Update is called once per frame
    void Update()
    {
        var playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();
        int playerScore = playerConfigs[PlayerIndex].PlayerScore;
        playerScoreText.SetText("Player " + (PlayerIndex + 1).ToString() + " score: " + playerScore);
    }
}
