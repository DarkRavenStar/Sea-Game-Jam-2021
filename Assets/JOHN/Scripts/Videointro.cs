using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Videointro : MonoBehaviour
{
    BGM test;
    // Start is called before the first frame update
    void OnEnable()
    {
        test = FindObjectOfType<BGM>();
        StartCoroutine(LoadScene());

       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator LoadScene()
    {
        if(test != null)
        {
            test.GetComponent<AudioSource>().volume = 0;
        }
        yield return new WaitForSecondsRealtime(55f);
        SceneLoad.instance.hasPlayIntro = true;
        SceneLoad.instance.LoadScene("Main");
        if (test != null)
        {
            test.GetComponent<AudioSource>().volume = 0.3f;
        }
    }
}
