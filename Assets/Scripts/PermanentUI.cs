using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PermanentUI : MonoBehaviour
{
    //PlayerStats
    public int cherries = 0;
    public int health = 5;
    public TextMeshProUGUI cherryText;
    public TextMeshProUGUI healthAmount;

    public static PermanentUI perm;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        //Singleton
        if(!perm)
        {
            perm = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Reset()
    {
        health = 5;
        cherries = 0;
        cherryText.text = cherries.ToString();
    }

    public void Fall()
    {
        health -= 1;
        cherries = 0;
        cherryText.text = cherries.ToString();
    }
}
