using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    // Call this method to restart the current scene
    public void RestartCurrentScene()
    {
        // Get active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Reload it
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
