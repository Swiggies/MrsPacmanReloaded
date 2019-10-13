using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => SceneManagement.Instance.LoadScene(sceneName));
    }
}

