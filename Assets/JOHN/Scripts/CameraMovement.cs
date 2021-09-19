using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector2 screenBound;
    private float objectWidth;
    private float objectHeight;

    public Transform[] players;

    Camera cam;

    float offset = 4.4f;

    public Vector3 midPoint = Vector3.zero;

    private bool hasDead = false;

    private float camheight = 20;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        //StartCoroutine(CheckBounds());
    }

    private void OnEnable()
    {
        hasDead = false;
    }
    private void OnDisable()
    {
        hasDead = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckDeadPlayer();
        if (hasDead == false)
        {
            midPoint = new Vector3(0, camheight, ((players[0].position.z + players[1].position.z) / 2) - offset);
            var temp = Mathf.Clamp(midPoint.z, -6.73f, 75.24f);
            cam.transform.position = new Vector3(midPoint.x, midPoint.y, temp);
            screenBound = Camera.main.ScreenToWorldPoint(midPoint);
        }
        else
        {
            if (UndeadPlayer() != null)
            {
                var lerp = Vector3.Lerp(new Vector3(0, camheight, cam.transform.position.z), new Vector3(0, camheight, UndeadPlayer().position.z), 0.3f);
                var temp = Mathf.Clamp(lerp.z, -6.73f, 75.24f);
                cam.transform.position = new Vector3(lerp.x, lerp.y, lerp.z);
            }
        }
    }

    
    void CheckDeadPlayer()
    {
        foreach (var ply in players)
        {
            if(ply.GetComponent<BasePlayer>().IsDead)
            {
                hasDead = true;
                return;
            }
        }
        hasDead = false;    
    }

    Transform UndeadPlayer()
    {
        Transform temp = null;

        foreach (var ply in players)
        {
            if(ply.GetComponent<BasePlayer>().IsDead == false)
            {
                temp = ply;
            }
        }

        return temp;
    }

    public bool CanMove(Vector3 origin, GameObject toCheck, out float playerPos)
    {
        float temp = 0;
        Vector3 pointOnScreen = cam.WorldToScreenPoint(toCheck.GetComponentInChildren<Renderer>().bounds.center);

        //Is in front
        if (pointOnScreen.z < 0)
        {
            playerPos = temp;
            return false;
        }

        //Is in FOV
        if ((pointOnScreen.y < 40) || (pointOnScreen.y > Screen.height - 40))
        {
            temp = pointOnScreen.y;
            playerPos = temp;
            return false;
        }
        playerPos = temp;
        return true;
    }
}
