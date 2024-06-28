using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    public void StartGame()
    {
        SceneManager.LoadScene("Scenes/SampleScene", LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync("Scenes/Main Menu",UnloadSceneOptions.None);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void GoToSettingsMenu()
    {
        SceneManager.LoadScene("SettingsMenu");
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Scenes/Main Menu", LoadSceneMode.Single);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
