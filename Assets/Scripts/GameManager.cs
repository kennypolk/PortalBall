using System.Collections.Generic;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
    public static List<Player> Players = new List<Player>();
    public static GameManager Instance = null;

    private void Awake()
    {
        Instance = this;
    }

    void Start ()
    {
        foreach (var player in Players)
        {
            player.Init();
        }
    }

    void Update ()
    {
    
    }
}
