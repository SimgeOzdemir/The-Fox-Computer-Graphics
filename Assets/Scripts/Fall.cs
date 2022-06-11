using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Fall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PermanentUI.perm.Fall();
            if (PermanentUI.perm.health <= 0)
            {
                PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();
                PermanentUI.perm.Reset();
                SceneManager.LoadScene("YouFailed");
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
