using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    float restartGameTimer = 3f;

    void Start()
    {
        EventHandler.instance.endGameDelegate += OnEndGameNotify;
    }

    void OnEndGameNotify()
    {
        StartCoroutine(RestartGame());
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(restartGameTimer);
		SceneManager.LoadScene(0);
	}
}
