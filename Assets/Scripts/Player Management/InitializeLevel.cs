using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.Windows;

public class InitializeLevel : MonoBehaviour
{
    [SerializeField] 
    private Transform[] playerSpawns;
    [SerializeField]
    private GameObject playerPrefab;

    public GameObject PlayerScorePrefab;

    // Start is called before the first frame update
    void Start()
    {
        var rootMenu = GameObject.Find("PlayersUI");
        var playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            var player = Instantiate(playerPrefab, playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);
            player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);

            var menu = Instantiate(PlayerScorePrefab, rootMenu.transform);
            menu.GetComponent<PlayerScoreUIController>().setPlayerIndex(playerConfigs[i].PlayerIndex);
        }

        GameObject.Find("EndScreenUI").GetComponent<Canvas>().enabled = false;
    }
}
