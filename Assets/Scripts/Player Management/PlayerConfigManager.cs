using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigManager : MonoBehaviour
{
    private List<PlayerConfig> playerConfigs;

    [SerializeField]
    private int MaxPlayers = 4;

    public static PlayerConfigManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("SINGLETON- Trying to create another instance");
        }
        else 
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfig>();
        }
    }
    public List<PlayerConfig> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    public void ReadyPlayer(int Index)
    {
        playerConfigs[Index].IsReady = true;
        if((playerConfigs.Count <= MaxPlayers && playerConfigs.Count > 1) && playerConfigs.All(p => p.IsReady == true))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("Player Joined " + pi.playerIndex);
        
        if(!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
        {
            pi.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfig(pi));
        }
        if(pi.user.pairedDevices.Count > 1)
        {
            if (pi.user.pairedDevices[0].name == "Mouse" || pi.user.pairedDevices[0].name == "Keyboard")
            {
                return;
            }
            for (int i = 1; i < pi.user.pairedDevices.Count; i++)
            {
                pi.user.UnpairDevice(pi.user.pairedDevices.Last());
            }
            
        }
            
    }
}

public class PlayerConfig
{
    public PlayerConfig(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }
    public PlayerInput Input { get; set; } 
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }
    
}
