using UnityEngine;
using UnityEngine.SceneManagement;  // Import the SceneManager class

public class SceneChanger : MonoBehaviour
{
    // This function will be called when the button is clicked
    public void ChangeScene(string sceneName)
    {
        // Load the scene by name
        SceneManager.LoadScene(sceneName);
    }
}
