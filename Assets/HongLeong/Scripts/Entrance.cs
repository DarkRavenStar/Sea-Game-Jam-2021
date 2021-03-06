using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : BaseInteractableObject
{
    [Header("Entrance Settings")]
    public Game gameManager;
    public Animation ownAnim;
    public Renderer[] doorRendererList;

    private List<GameObject> mPlayerList;
    private bool mIsDoorOpened;

    protected override void Start()
    {
        base.Start();
        mPlayerList = new List<GameObject>();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        BasePlayer player = other.GetComponent<BasePlayer>();
        if (player != null)
        {
            GameObject tempPlayer = mPlayerList.Find((x) => x.GetInstanceID() == player.gameObject.GetInstanceID());
            if (tempPlayer == null)
            {
                mPlayerList.Add(player.gameObject);
                //add player action click callback
                player.OnInteract += ActionCall;
            }
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        BasePlayer player = other.GetComponent<BasePlayer>();
        if (player != null)
        {
            GameObject tempPlayer = mPlayerList.Find((x) => x.GetInstanceID() == player.gameObject.GetInstanceID());
            if (tempPlayer != null)
            {
                mPlayerList.Remove(tempPlayer);
                //add player action click callback
                player.OnInteract -= ActionCall;
            }
        }
    }

    protected override void ActionCall()
    {
        if (mIsDoorOpened) return;

        if (mPlayerList.Count == 2)
        {
            //call game manager end game
            if (gameManager.WinScene())
            {
                mIsDoorOpened = true;
                ownAnim.Play("entrance-open");
            }
        }
    }

    public void ChangeDoorColor()
    {
        for (int i = 0; i < doorRendererList.Length; i++)
        {
            doorRendererList[i].material.color = Color.green;
        }
    }
}
