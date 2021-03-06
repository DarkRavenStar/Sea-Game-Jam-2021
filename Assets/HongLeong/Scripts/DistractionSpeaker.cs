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

    [Header("Switch")]
    public Animation ownAnim;

    [Header("Sound")]
    public SpeakerAnimation speakerAnim;

    private List<GameObject> mSoldierList;
    private bool isAnimPlaying = false;

    protected override void Start()
    {
        base.Start();
        mSoldierList = new List<GameObject>();

        if(speakerAnim)
        {
            speakerAnim.SetAnimationEndCb(AnimationEndCallback);
        }

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
            BasePlayer player = other.GetComponent<BasePlayer>();
            if (player != null)
            {
                //set player action click callback
                player.OnInteract += ActionCall;
            }
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        //which player can detect ( WIP )
        if (speakerType == SpeakerType.SWITCH)
        {
            //get player
            BasePlayer player = other.GetComponent<BasePlayer>();
            if (player != null)
            {
                //remove player action click callback
                player.OnInteract -= ActionCall;
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
            if (isAnimPlaying) return;

            speaker?.ActionCall();
        }
        else if (speakerType == SpeakerType.SPEAKER)
        {
            Debug.LogWarning("[DistractionSpeaker] SpeakerType.SPEAKER");
            isAnimPlaying = true;
            ownAnim.Play("bell-swing");

            //announce to all soldier in list
            foreach (GameObject go in mSoldierList)
            {
                GuardAI gai = go.GetComponent<GuardAI>();

                if (gai != null)
                {
                    gai.SetCurrentTarget(this.transform);
                }
            }
        }
    }

    private void AnimationEndCallback()
    {
        isAnimPlaying = false;
    }
}
