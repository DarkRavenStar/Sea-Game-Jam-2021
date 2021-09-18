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
            if(!player)
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
            if (!player)
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
        Debug.Log("Adding");

        if (!deadPlayers.Contains(ply))
            deadPlayers.Add(ply);

        if(deadPlayers.Count == Players.Length)
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
        Debug.Log("Game Over");

    }

    public virtual void DeathEvent(GameObject player)
    {
        List<BasePlayer> undeads = new List<BasePlayer>();
        BasePlayer[] temp = FindObjectsOfType<BasePlayer>();

        for (int i = 0; i < temp.Length; i++)
        {
            temp[i].OnDeath(this.gameObject);
            //if (temp[i].isDead == false)
            //{
            //    undeads.Add(temp[i]);

            //}
            //else
            //{
            //    continue;
            //}
        }
        //foreach (var undead in undeads)
        //{
        //    undead.OnDeath(this.gameObject);
        //}
    }
}
