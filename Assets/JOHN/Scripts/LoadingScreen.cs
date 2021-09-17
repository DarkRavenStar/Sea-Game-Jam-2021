using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI progressText;

    [SerializeField]
    private Slider progressBar;

    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void UpdateProgress(float progress)
    {
        progressBar.value = progress;
        progressText.text = (int)(progress * 100f) + "%";
    }
}
