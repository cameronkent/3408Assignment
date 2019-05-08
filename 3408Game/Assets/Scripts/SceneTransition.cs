using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Animator animator;
    public string sceneName;
    public bool levelComplete;
    public Canvas uiCanvas;
    
    // Update is called once per frame

   
    void Update()
    {
        if (levelComplete)
        {
            uiCanvas.enabled = false;
            SceneManager.LoadScene(sceneName);
            StartCoroutine(LoadScene());
        }
        else
        {
            uiCanvas.enabled = true;
        }
    }

    IEnumerator LoadScene()
    {
        animator.SetTrigger("end");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneName);
    }
}
