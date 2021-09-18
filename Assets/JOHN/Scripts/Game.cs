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
    private bool mHasCrown = false;
    // Start is called before the first frame update
    void OnEnable()
    {
        foreach (var player in Players)
        {
            if (player)
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
            if (player)
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
        if (!deadPlayers.Contains(ply))
            deadPlayers.Add(ply);

        if (deadPlayers.Count == Players.Length)
        {
            StartCoroutine(Co_GameOver());

        }
    }

    private void RemoveDeath(GameObject ply)
    {
        if (deadPlayers.Contains(ply))
            deadPlayers.Remove(ply);
    }

    IEnumerator Co_GameOver()
    {
        yield return new WaitForSeconds(1.0f);//Delay
        GameOver();

    }
    private void GameOver()
    {
        frontEndUI._PlayAnimation(frontEndUI.GetComponent<CanvasGroup>(), 1, 0.4f, null, true);
    }


    public void LoadScene()
    {
        SceneLoad.instance.LoadScene("Start");
    }

    public bool WinScene()
    {
        if (mHasCrown)
        {
            //go to win UI
            SceneLoad.instance?.LoadScene("End");
            return true;
        }

        return false;
    }

    public void TakeCrown()
    {
        mHasCrown = true;
    }

}
