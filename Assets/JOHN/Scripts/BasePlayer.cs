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

    private System.Action<string> OnDeath = delegate { };

    private void OnEnable()
    {
        OnDeath += Testing;
    }
    public virtual void Update()
    {
        //if(playerHealth < 1)
        //{
        //    Death();
        //}

        if(Input.GetKeyDown(KeyCode.A))
        {
            if(this.tag == "Player1")
            {
                Death();
            }
        }
    }
    public virtual void Revive()
    {

    }

    public virtual void Death()
    {
        //StartCoroutine(DeathRoutine());

        //Notify other player
        List<BasePlayer> undeads = new List<BasePlayer>();
        BasePlayer[] temp = FindObjectsOfType<BasePlayer>();
        string tempDeathTag = "";
         
        for (int i =0; i < temp.Length; i ++)
        {
            if (temp[i] == this)
                tempDeathTag = temp[i].tag;

            if(temp[i] != this)
            {
                undeads.Add(temp[i]);
            }
        }
        foreach(var undead in undeads)
        {
            undead.OnDeath(tempDeathTag);
        }
    }

    private void teleport(Transform pos)
    {
        transform.position = pos.position;
    }

    private IEnumerator DeathRoutine()
    {
        ///DO death routine here, vulnerable? invulnerable? fall down animation?
        ///

        yield return new WaitForSeconds(5.0f);
        teleport(JailPosition.transform);
    }

    private void Testing(string tag)
    {
        Debug.Log(tag + "TAG");

    }
}
