using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpeakerType
{
    NONE,
    SWITCH,
    SPEAKER
}

public enum PlayerType
{
    NONE,
    PLAYER_1,
    PLAYER_2
}

/// <summary>
/// HL - Act either Switch or Speaker behaviour
/// Switch is for player to trigger
/// Speaker is for send signal to soldier
/// </summary>
public class DistractionSpeaker : BaseInteractableObject
{
    public SpeakerType speakerType = SpeakerType.NONE;

    [Header("Switch Act Settings")]
    public DistractionSpeaker speaker;
    public PlayerType playerDetect = PlayerType.NONE;

    [Header("Speaker Act Settings")]
    public List<DistractionSpeakerArea> areaList;
    private List<GameObject> mSoldierList;

    protected override void Start()
    {
        base.Start();
        mSoldierList = new List<GameObject>();

        for (int i = 0; i < areaList.Count; i++)
        {
            areaList[i].addSoldierCallback += AddSoldier;
            areaList[i].removeSoldierCallback += RemoveSoldier;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        //which player can detect ( WIP )
        if (speakerType == SpeakerType.SWITCH)
        {
            //get player
            if (playerDetect == PlayerType.PLAYER_1)
            {
                if (other.tag.Equals("Player1"))
                {
                    //set player action click callback
                }
            }
            else if (playerDetect == PlayerType.PLAYER_2)
            {
                if (other.tag.Equals("Player2"))
                {
                    //set player action click callback
                }
            }

        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        //which player can detect ( WIP )
        if (speakerType == SpeakerType.SWITCH)
        {
            //get player
            if (playerDetect == PlayerType.PLAYER_1)
            {
                if (other.tag.Equals("Player1"))
                {
                    //remove player action click callback
                }
            }
            else if (playerDetect == PlayerType.PLAYER_2)
            {
                if (other.tag.Equals("Player2"))
                {
                    //remove player action click callback
                }
            }
        }
    }

    private void AddSoldier(GameObject _soldierGo)
    {
        // store soldier when enter the it's collider
        GameObject soldier = mSoldierList.Find((x) => x.GetInstanceID() == _soldierGo.GetInstanceID());
        if (!soldier)
        {
            mSoldierList.Add(_soldierGo);
        }
    }

    private void RemoveSoldier(GameObject _soldierGo)
    {
        //set player action click callback
        GameObject soldier = mSoldierList.Find((x) => x.GetInstanceID() == _soldierGo.GetInstanceID());
        if (soldier)
        {
            mSoldierList.Remove(soldier);
        }
        else
        {
            Debug.LogWarning("[DistractionSpeakerArea] Cannot find same soldier id to remove.");
        }
    }

    protected override void ActionCall()
    {
        if (speakerType == SpeakerType.SWITCH)
        {
            speaker?.ActionCall();
        }
        else if (speakerType == SpeakerType.SPEAKER)
        {
            //announce to all soldier in list
        }
    }
}
