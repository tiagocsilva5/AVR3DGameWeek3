using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Start Scene")]
    public string startScene = "Scene_1"; 

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // If the current scene is not the start scene, load it
        if (SceneManager.GetActiveScene().name != startScene)
        {
            SceneManager.LoadScene(startScene);
        }
    }
}