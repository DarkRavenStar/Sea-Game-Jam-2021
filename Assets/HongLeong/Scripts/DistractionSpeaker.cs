using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpeakerType
{
    NONE,
    SWITCH,
    SPEAKER
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

    [Header("Speaker Act Settings")]
    public List<DistractionSpeakerArea> areaList;
    private List<GameObject> soldierList;

    protected override void Start()
    {
        base.Start();
        soldierList = new List<GameObject>();

        for (int i = 0; i < areaList.Count; i++)
        {
            areaList[i].addSoldierCallback += AddSoldier;
            areaList[i].removeSoldierCallback += RemoveSoldier;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (speakerType == SpeakerType.SWITCH)
        {
            //get player
            if (other.tag.Equals("player"))
            {
                //set player action click callback
            }
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (speakerType == SpeakerType.SWITCH)
        {
            if (other.tag.Equals("player"))
            {
                //remove player action click callback
            }
        }
    }

    private void AddSoldier(GameObject _soldierGo)
    {
        // store soldier when enter the it's collider
        GameObject soldier = soldierList.Find((x) => x.GetInstanceID() == _soldierGo.GetInstanceID());
        if (!soldier)
        {
            soldierList.Add(_soldierGo);
        }
    }

    private void RemoveSoldier(GameObject _soldierGo)
    {
        //set player action click callback
        GameObject soldier = soldierList.Find((x) => x.GetInstanceID() == _soldierGo.GetInstanceID());
        if (soldier)
        {
            soldierList.Remove(soldier);
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
