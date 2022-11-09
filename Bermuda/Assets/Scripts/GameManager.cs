using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public LevelManager CurrentLevelManager;

    public int Level;

    public static GameManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Lose()
    {
        UIManager.instance.ShowLoseMenu();
        CurrentLevelManager.Player.Lose();
    }
    public void LoadLevel(int num)
    {
        SceneManager.LoadScene(num);
    }
    private void OnLevelWasLoaded(int level)
    {
        Level = level;
        CurrentLevelManager = FindObjectOfType<LevelManager>();
    }
}