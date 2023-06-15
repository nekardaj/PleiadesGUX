using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class FadeManager : MonoBehaviour
{
    public Animator animator;
    public VideoPlayer video;

    private int indexToLoad;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            StartCoroutine(FadeIntoMenu());
        }
    }

    public void FadeToLevel(int index)
    {
        indexToLoad = index;
        animator.SetTrigger("FadeOut");
    }

    public void FadeOut()
    {
        indexToLoad = -1;
        animator.SetTrigger("FadeOut");
    }

    public void FadeIn()
    {
        animator.SetTrigger("FadeIn");
    }

    public void OnFadeComplete()
    {
        if (indexToLoad >= 0)
        {
            SceneManager.LoadScene(indexToLoad);
        }
        else
        {
            GetComponent<EndManager>().SetEndPicture();
            FadeIn();
        }
    }

    private IEnumerator FadeIntoMenu()
    {
        yield return new WaitForSeconds(float.Parse(video.length.ToString()));
        FadeToLevel(0);
    }
}
