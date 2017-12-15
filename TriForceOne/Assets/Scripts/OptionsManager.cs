using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class OptionsManager : MonoBehaviour {
    
    public void GoBack()
    {
        SceneManager.LoadScene(0);
    }
}
