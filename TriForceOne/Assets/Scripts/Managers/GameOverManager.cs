using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public int maxLives;
    public GameObject gameOverScreen;

    int lives;
    bool gameOver;
    bool restartEnabled;
    Text livesText;

    void Start()
    {
        gameOver = false;
        restartEnabled = false;
        livesText = GameObject.Find("LivesText").GetComponent<Text>();
        lives = maxLives;
        livesText.text = "Lives: " + lives;
    }

    public void LoseLife()
    {
        lives--;
        if (lives <= 0)
        {
            gameOver = true;
        }
        livesText.text = "Lives: " + lives;
    }

    void Update()
    {
        if (gameOver)
        {
            restartEnabled = true;
            gameOverScreen.SetActive(true);
            GameObject waveAmount = GameObject.Find("WavesSurvivedAmount");
            waveAmount.GetComponent<Text>().text = GetComponent<SpawnManager>().GetWave().ToString();
            Time.timeScale = 0;
        }
        if (restartEnabled && Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ändrad från att ladda om den specifika game scenen till den nuvarande scenen när man vill starta om. - Alexander W
        }
    }

    public void TimedOut() // Denna metoden kallas när en korutin i SpawnManager ser efter om tiden har runnit ut innan alla robotar är döda eller har tagit sig igenom banan under en våg. Ta bort? - Alexander W
    {
       // gameOver = true;
    }

	public bool GameOver
	{
		get { return gameOver; }
	}
}