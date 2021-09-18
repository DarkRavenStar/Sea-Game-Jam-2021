using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.DemiLib;
using DG.Tweening;

public enum PlayerAbility
{
    NONE,
    DISPEL,
    INFUSE
}

public class BasePlayer : MonoBehaviour
{
    public PlayerAbility playerAbility = PlayerAbility.NONE;

    public float playerHealth = 1;

    public float movementSpeed = 10;

    [SerializeField]
    private Transform JailPosition;

    protected bool isDead = false;

    [SerializeField]
    protected player Player;
    protected enum player
    {
        player1,
        player2
    }

    public System.Action<GameObject> OnDeath = delegate { };

    public System.Action<GameObject> OnRevive = delegate { };

    public System.Action OnInteract = delegate { };

    protected virtual void OnEnable()
    {
        OnDeath += OnDeathPlayer;
    }

    protected virtual void OnDisable()
    {
        OnDeath -= OnDeathPlayer;
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
            if (Player == player.player1)
                Damage();
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            if (Player == player.player1)
                this.OnInteract?.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(Player == player.player2)
                this.OnInteract?.Invoke();
        }
    }
    public virtual void Revive()
    {
        playerHealth = 1;
        isDead = false;
        OnRevive(this.gameObject);
    }

    private void teleport(Transform pos)
    {
        transform.position = pos.position;
    }
    public void Damage()
    {
        if(this.playerHealth > 0)
        this.playerHealth -= 1;
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
