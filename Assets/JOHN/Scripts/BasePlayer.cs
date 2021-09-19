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
    [SerializeField]
    private Transform JailPosition2;

    protected bool isDead = false;
    public bool IsDead { get { return isDead; } private set {; } }

    private bool canRevive = false;

    private BasePlayer playerInTrigger;

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
        OnRevive += OnRevivedPlayer;
    }

    protected virtual void OnDisable()
    {
        OnDeath -= OnDeathPlayer;
        OnRevive -= OnRevivedPlayer;
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

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (Player == player.player2)
                Damage();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (Player == player.player1)
                Damage();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Player == player.player1)
            {
                this.OnInteract?.Invoke();
            }
            if(canRevive && Player == player.player1)
            {
                if(playerInTrigger)
                    Revive(playerInTrigger);
            }
        }
        if(Input.GetKeyDown(KeyCode.RightControl))
        {
            if (Player == player.player2)
            {
                this.OnInteract?.Invoke();
            }
            if(canRevive && Player == player.player2)
            {
                if(playerInTrigger)
                    Revive(playerInTrigger);
            }
        }
    }

    /// <summary>
    /// Call revive
    /// </summary>
    public virtual void Revive(BasePlayer ply)
    {
        ply.playerHealth = 1;
        ply.isDead = false;
        OnRevive(ply.gameObject);
    }

    private void teleport(Transform pos, GameObject ply)
    {
        ply.transform.position = pos.position;
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
            if(this.gameObject != temp[i].gameObject)
                temp[i].OnDeath(this.gameObject);
        }
    }

    private IEnumerator DeathRoutine(GameObject ply)
    {
        ply.GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezePosition;
        ply.GetComponent<CapsuleCollider>().enabled = false;
        ply.GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(3f);
        if(ply.GetComponent<BasePlayer>().Player == player.player1)
        {
            teleport(JailPosition.transform, ply);
        }
        else
        {
            teleport(JailPosition2.transform, ply);
        }

        ply.GetComponent<CapsuleCollider>().enabled = true;
        ply.GetComponent<Rigidbody>().isKinematic = false;
    }

    protected virtual void OnDeathPlayer(GameObject deadPlayer)
    {

        //Listener for deadplayer
        StartCoroutine(DeathRoutine(deadPlayer));
        Debug.Log(deadPlayer.name + "is dead");
    }
    protected virtual void OnRevivedPlayer(GameObject ply)
    {
        ply.GetComponent<Rigidbody>().constraints &= ~(RigidbodyConstraints.FreezePosition);
        //Listener for revived ply
        Debug.Log(ply.name + "is revived");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent)
        {
            if (other.transform.parent.GetComponent<BasePlayer>())
            {
                if (other.transform.parent.GetComponent<BasePlayer>().isDead == true)
                {
                    canRevive = true;
                    if (playerInTrigger == null)
                    {
                        playerInTrigger = other.transform.parent.GetComponent<PlayerMovement>() as BasePlayer;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent)
        {
            if (other.transform.parent.GetComponent<BasePlayer>())
            {
                canRevive = false;
                playerInTrigger = null;
            }
        }
    }
}
