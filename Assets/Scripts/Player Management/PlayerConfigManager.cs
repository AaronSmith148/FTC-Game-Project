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

    [SerializeField]
    public string sceneToLoad = "SampleScene";

    private Dictionary<string, int> mapVotes = new Dictionary<string, int>();

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
            mapVotes.Add("SampleScene", 0);
            mapVotes.Add("LevelOne", 0);
            mapVotes.Add("LevelTwo", 0);
        }
    }
    public List<PlayerConfig> GetPlayerConfigs()
    {
        return playerConfigs;
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

    public void SetLevelChoice(int index, string levelChoice)
    {
        playerConfigs[index].LevelChoice = levelChoice;
    }

    private void LevelSelectVote()
    {
        if(playerConfigs.All(p => p.LevelChoice != null))
        {
            foreach(PlayerConfig player in playerConfigs)
            {
                int votes;
                mapVotes.TryGetValue(player.LevelChoice, out votes);
                mapVotes[player.LevelChoice] = ++votes;
            }

            sceneToLoad = mapVotes.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            Debug.Log("Voted Scene:" + sceneToLoad);

        }


    }

    public void ReadyPlayer(int Index)
    {
        playerConfigs[Index].IsReady = true;
        if ((playerConfigs.Count <= MaxPlayers && playerConfigs.Count > 1) && playerConfigs.All(p => p.IsReady == true))
        {
            LevelSelectVote();
            Debug.Log("Loading Scene:" + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
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
    public int PlayerScore { get; set; }
    public string LevelChoice { get; set; }

}
