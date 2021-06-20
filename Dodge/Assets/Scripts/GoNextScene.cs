using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoNextScene : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("Level_1");
    }

}
