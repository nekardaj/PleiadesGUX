using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public Animator animator;

    private int indexToLoad;

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
        if (indexToLoad > 0)
        {
            SceneManager.LoadScene(indexToLoad);
        }
        else
        {
            GetComponent<EndManager>().SetEndPicture();
            FadeIn();
        }
    }
}
