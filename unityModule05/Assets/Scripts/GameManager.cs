using System.Threading.Tasks;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Fader fader;
    public PlayerController player;
    public UIDocument errorMessage;
    public int itemCount = 0;
    public int totalCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async Awaitable Start()
    {
        if (instance != null)
        {
            instance.AssignInheritParameter(this);
            instance.Init();
            await fader.FadeIn();
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        Init();
        await fader.FadeIn();
    }

    void Init()
    {
        player.defeated.AddListener(OnDefeated);
        errorMessage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene (SceneManager.GetActiveScene().name);
        }
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

    void AssignInheritParameter(GameManager other)
    {
        itemCount = other.itemCount;
        fader = other.fader;
        player = other.player;
    }

    public void AddItemCount()
    {
        itemCount += 1;
        totalCount += 1;
    }

    public bool CanClear()
    {
        return itemCount >= 5;
    }

    public void Clear()
    {
        return;
    }
    
    public async Awaitable NotifyError()
    {
        errorMessage.gameObject.SetActive(true);
        for (int i = 0; i < 300; i++)
        {
            await Awaitable.NextFrameAsync();
        }
        errorMessage.gameObject.SetActive(false);
    }
}
