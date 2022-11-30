using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField]private float _levelLoadDelay = 2f;
    [SerializeField] private float _levelExitSlowMoFactor = 0.2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>()!=null)
        {
            StartCoroutine(GoToNextLevel());
        }
    }


    private IEnumerator GoToNextLevel()
    {
        Time.timeScale = _levelExitSlowMoFactor;
        yield return new WaitForSecondsRealtime(_levelLoadDelay);
        Time.timeScale = 1f;
        var currenSceneIndex = SceneManager.GetActiveScene().buildIndex;
       
        SceneManager.LoadScene(currenSceneIndex+1);
        //SceneManager.UnloadSceneAsync(currenSceneIndex);
    }

}
