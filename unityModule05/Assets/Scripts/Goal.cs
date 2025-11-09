using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public string nextScene;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (GameManager.instance.CanClear())
            {
                GameManager.instance.Clear();
                SceneManager.LoadScene(nextScene);
            }
            else
            {
                _ = GameManager.instance.NotifyError();
            }
        }
    }
}
