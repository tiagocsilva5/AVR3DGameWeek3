using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string startScene = "Scene_1";
    public string loadScene = "Scene_2";

    [Header("UI")]
    public GameObject blackScreen;
    public TMP_Text messageText;
    public float transitionDuration = 5f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Automatically find UI in the new scene
        blackScreen = GameObject.Find("FadeScreen");

        if (blackScreen != null)
        {
            messageText = blackScreen.GetComponentInChildren<TMP_Text>();
            blackScreen.SetActive(false);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(startScene);
    }

    public void PlayerReachedGoal(string message, string loadScene)
    {
        StartCoroutine(TransitionToLoadScene(message, loadScene));
    }

    IEnumerator TransitionToLoadScene(string message, string loadScene)
    {
        if (blackScreen != null)
        {
            blackScreen.SetActive(true);
            messageText.text = message;
        }

        yield return new WaitForSeconds(transitionDuration);

        SceneManager.LoadScene(loadScene);

        yield return null;

        if (blackScreen != null)
            blackScreen.SetActive(false);
    }
}