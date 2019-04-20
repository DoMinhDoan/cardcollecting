using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader: MonoBehaviour {

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
