using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene loading

public class SceneChanger : MonoBehaviour
{
    // This function can be called from the button's OnClick event
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene("JaniTest");
    }
}
