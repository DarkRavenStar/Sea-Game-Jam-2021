using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    public CanvasGroup tutorial;
    public BaseUI baseUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string sceneName)
    {
        if (SceneLoad.instance.hasPlayIntro == false)
        {
            SceneLoad.instance.LoadScene(sceneName);
        }
        else
        {
            SceneLoad.instance.LoadScene("Main");
        }
    }

    public void ToggleTutorial()
    {
        if(tutorial.alpha > 0)//close this
            baseUI._PlayAnimation(tutorial, 0, 0.4f, null, false);
        else if (tutorial.alpha < 1)
            baseUI._PlayAnimation(tutorial, 1, 0.4f, null, true);
    }
}
