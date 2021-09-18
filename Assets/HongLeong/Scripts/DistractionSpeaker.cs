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
    public PlayerAbility playerDetect = PlayerAbility.NONE;

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
            if (playerDetect == PlayerAbility.INFUSE)
            {
                BasePlayer player = other.GetComponent<BasePlayer>();
                if (player != null && player.playerAbility == PlayerAbility.INFUSE)
                {
                    //set player action click callback
                    player.OnInteract += ActionCall;
                }
            }
            else if (playerDetect == PlayerAbility.DISPEL)
            {
                BasePlayer player = other.GetComponent<BasePlayer>();
                if (player != null && player.playerAbility == PlayerAbility.DISPEL)
                {
                    //set player action click callback
                    player.OnInteract += ActionCall;
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
            if (playerDetect == PlayerAbility.INFUSE)
            {
                BasePlayer player = other.GetComponent<BasePlayer>();
                if (player != null && player.playerAbility == PlayerAbility.INFUSE)
                {
                    //remove player action click callback
                    player.OnInteract -= ActionCall;
                }
            }
            else if (playerDetect == PlayerAbility.DISPEL)
            {
                BasePlayer player = other.GetComponent<BasePlayer>();
                if (player != null && player.playerAbility == PlayerAbility.DISPEL)
                {
                    //remove player action click callback
                    player.OnInteract -= ActionCall;
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
            Debug.LogWarning("[DistractionSpeaker] SpeakerType.SPEAKER");
            //announce to all soldier in list
            foreach (GameObject go in mSoldierList)
            {
                GuardAI gai = go.GetComponent<GuardAI>();
                
                if(gai != null)
                {
                    gai.SetCurrentTarget(this.transform);
                }
            }
        }
    }
}
