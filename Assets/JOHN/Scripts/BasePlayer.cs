using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.DemiLib;
using DG.Tweening;


public class BasePlayer : MonoBehaviour
{
    public float playerHealth = 1;

    public float movementSpeed = 10;

    [SerializeField]
    private Transform JailPosition;

    private bool isDead = false;

    [SerializeField]
    protected player Player;
    protected enum player
    {
        player1,
        player2
    }

    private System.Action<GameObject> OnDeath = delegate { };

    protected virtual void OnEnable()
    {
        OnDeath += OnDeathPlayer;
    }
    public virtual void Update()
    {
        if (this.playerHealth < 1)
        {
            if(!isDead)
            {
                isDead = true;
                DeathEvent(this.gameObject);
            }
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            if(Player == player.player2)
                Damage();
        }
    }
    public virtual void Revive()
    {
        playerHealth = 1;
        isDead = false;
    }

    private void teleport(Transform pos)
    {
        transform.position = pos.position;
    }

    private void Damage()
    {
        this.playerHealth -= 1;
    }

    public virtual void DeathEvent(GameObject player)
    {
        List<BasePlayer> undeads = new List<BasePlayer>();
        BasePlayer[] temp = FindObjectsOfType<BasePlayer>();

        for (int i = 0; i < temp.Length; i++)
        {

            if (temp[i].isDead == false)
            {
                undeads.Add(temp[i]);

            }
            else
            {
                continue;
            }
        }
        foreach (var undead in undeads)
        {
            undead.OnDeath(this.gameObject);
        }
    }

    private IEnumerator DeathRoutine()
    {
        ///DO death routine here, vulnerable? invulnerable? fall down animation?
        ///

        yield return new WaitForSeconds(5.0f);
        teleport(JailPosition.transform);
    }

    public void OnDeathPlayer(GameObject deadPlayer)
    {
        //Listener for deadplayer
        Debug.Log(deadPlayer.name + "is dead");


    }
}
