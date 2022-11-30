using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    
    
    public void StartFirstScene()
    {
        StartScene(1);
    }
    
    
    
    private void StartScene(int num)
    {
        Scene scene = SceneManager.GetSceneByBuildIndex(num);
        if (scene!=null)
        {
            SceneManager.LoadScene(num);        
        }
    }
}
