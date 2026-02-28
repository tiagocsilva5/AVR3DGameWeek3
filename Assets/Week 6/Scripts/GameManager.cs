using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Scenes")]
    [SerializeField] private string startScene = "Scene_1";

    [Header("Fade UI (Child of GameManager)")]
    [SerializeField] private GameObject fadeScreen;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private float transitionDuration = 3f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Make sure fade starts hidden
        if (fadeScreen != null)
            fadeScreen.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(startScene);
    }

    public void LoadSceneWithMessage(string sceneName, string message)
    {
        StartCoroutine(Transition(sceneName, message));
    }

    private IEnumerator Transition(string sceneName, string message)
    {
        if (fadeScreen != null)
        {
            fadeScreen.SetActive(true);

            if (messageText != null)
                messageText.text = message;
        }

        yield return new WaitForSeconds(transitionDuration);

        SceneManager.LoadScene(sceneName);

        yield return null;

        if (fadeScreen != null)
            fadeScreen.SetActive(false);
    }
}
