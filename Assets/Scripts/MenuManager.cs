using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public InputField inputField;

    public class UserName{
        public static string userName = "";
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame(){
        UserName.userName = inputField.text; 

        SceneManager.LoadScene(1);
    }

    public void ExitGame(){
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else 
            Application.Quit();
        #endif
    }
}
