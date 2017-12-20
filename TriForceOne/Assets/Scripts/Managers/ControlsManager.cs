using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ControlsManager : MonoBehaviour
{
    public GameObject[] pages;
    public Button backButton;
    public Button nextButton;

    int maxPage;
    int page;

    void Start()
    {
        maxPage = pages.Length - 1;
        page = 0;
        pages[page].SetActive(true);
    }

    public void GoToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Next()
    {
        if (page < maxPage)
        {
            pages[page].SetActive(false);
            pages[page + 1].SetActive(true);
            if (page >= maxPage -1)
            {
                nextButton.gameObject.SetActive(false);
            }
            if (!backButton.gameObject.activeInHierarchy)
            {
                backButton.gameObject.SetActive(true);
            }
            page++;
        }
    }

    public void Back()
    {
        if (page > 0)
        {
            pages[page].SetActive(false);
            pages[page - 1].SetActive(true);
            if (page <= 1)
            {
                backButton.gameObject.SetActive(false);
            }
            if (!nextButton.gameObject.activeInHierarchy)
            {
                nextButton.gameObject.SetActive(true);
            }
            page--;
        }
    }
}
