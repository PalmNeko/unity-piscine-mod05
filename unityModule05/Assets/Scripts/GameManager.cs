using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Fader fader;
    public PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async Awaitable Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        player.defeated.AddListener(OnDefeated);
        await fader.FadeIn();
    }

    void OnDefeated()
    {
        _ = Replay(); 
    }

    async Awaitable Replay()
    {
        await fader.FadeOut();
        player.Respawn();
        await fader.FadeIn();
    }
}
