using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    private BasePlayer[] Players;

    [SerializeField]
    private BaseUI frontEndUI;

    private List<GameObject> deadPlayers = new List<GameObject>();
    // Start is called before the first frame update
    void OnEnable()
    {
        foreach(var player in Players)
        {
            if(player)
            {
                player.OnDeath += AddDeath;
                player.OnRevive += RemoveDeath;
            }
        }
    }

    private void OnDisable()
    {
        foreach (var player in Players)
        {
            if (player)
            {
                player.OnDeath -= AddDeath;
                player.OnRevive -= RemoveDeath;
            }
        }
        deadPlayers = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void AddDeath(GameObject ply)
    {
        if (!deadPlayers.Contains(ply))
            deadPlayers.Add(ply);

        Debug.Log("ADS");

        Debug.Log(deadPlayers.Count + "Deadplayers " + Players.Length + "players");

        if (deadPlayers.Count == Players.Length)
        {
            GameOver();
        }
    }

    private void RemoveDeath(GameObject ply)
    {
        if(deadPlayers.Contains(ply))
            deadPlayers.Remove(ply);
    }

    private void GameOver()
    {
        frontEndUI._PlayAnimation(frontEndUI.GetComponent<CanvasGroup>(), 1, 0.4f, null, true);
    }

}
