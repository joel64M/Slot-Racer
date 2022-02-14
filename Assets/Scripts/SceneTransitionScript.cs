using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionScript : MonoBehaviour
{
    public static SceneTransitionScript instance;

    [SerializeField] Animator anim;
    [SerializeField] float transitionTime = 1f;
    [SerializeField] Slider progressSlider;
    private void Awake()
    {
        if (instance != this && instance!=null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void StartransitionTo(int sceneIndex)
    {
        StartCoroutine(SceneTransitionCoroutine(sceneIndex));
    }
    public void StartransitionTo(string sceneName)
    {
        StartCoroutine(SceneTransitionCoroutine(sceneName));
    }

    IEnumerator SceneTransitionCoroutine(int sceneIndex)
    {
        anim.SetTrigger("Start");
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        yield return new WaitForSeconds(transitionTime);

        //operation.allowSceneActivation = true;
        while (!operation.isDone)
        {
            progressSlider.value = Mathf.Clamp01(operation.progress / 0.9f);
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
        //operation.allowSceneActivation = true;

    }

    IEnumerator SceneTransitionCoroutine(string sceneName)
    {
        anim.SetTrigger("Start");
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        yield return new WaitForSeconds(transitionTime);

        while (!operation.isDone)
        {
            progressSlider.value = Mathf.Clamp01(operation.progress / 0.9f);

            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }

    }
}
