using System.Text.RegularExpressions;
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
    public BackTitle backTitleUI;
    public int itemCount = 0;
    public int totalCount = 0;

    private bool loadGame = false;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var regex = new Regex("Stage");
        if (regex.IsMatch(scene.name) && loadGame == false)
        {
            _ = Respawn();
        }
        loadGame = false;
    }
    
    async Awaitable Respawn()
    {
        itemCount = 0;
        PlayerVisible(true);
        player.Respawn();
        await fader.FadeIn();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Boot();
    }

    void Boot()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        Init();
    }
    
    void Init()
    {
        errorMessage.gameObject.SetActive(true);
        player.gameObject.SetActive(true);
        player.defeated.AddListener(OnDefeated);
        errorMessage.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
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
        PlayerVisible(true);
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
        PlayerVisible(false);
        return;
    }

    public void PlayerVisible(bool visible)
    {
        player.gameObject.SetActive(visible);
        if (visible == false)
            player.StopMovement();
        else if (visible == true)
            player.StartMovement();
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

    public async Awaitable LoadGame()
    {
        await fader.FadeOut();
        PlayerVisible(true);
        LoadData();
        backTitleUI.gameObject.SetActive(true);
        loadGame = true;
        await fader.FadeIn();
    }
    
    public void LoadData()
    {
        if (PlayerPrefs.GetInt("HasSave") != 1)
            return;
        var stageName = PlayerPrefs.GetString("LastStage");
        SceneManager.LoadScene(stageName);
        Debug.Log("OnLoad");
        Vector2 position = new Vector2(
            PlayerPrefs.GetFloat("PositionX"),
            PlayerPrefs.GetFloat("PositionY"));
        player.transform.position = position;
        player.hp = PlayerPrefs.GetFloat("HP");
        itemCount = PlayerPrefs.GetInt("ItemCount");
        totalCount = PlayerPrefs.GetInt("TotalCount");
    }

    public async Awaitable NewGame()
    {
        await fader.FadeOut();
        ResetData();
        SceneManager.LoadScene("Stage1");
        backTitleUI.gameObject.SetActive(true);
        PlayerVisible(true);
        player.Respawn();
    }

    public void ResetData()
    {
        return;
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("HasSave", 1);
        PlayerPrefs.SetFloat("PositionX", player.transform.position.x);
        PlayerPrefs.SetFloat("PositionY", player.transform.position.y);
        PlayerPrefs.SetFloat("HP", player.hp);
        PlayerPrefs.SetInt("ItemCount", itemCount);
        PlayerPrefs.SetInt("TotalCount", totalCount);
        var regex = new Regex("Stage");
        var stageName = SceneManager.GetActiveScene().name;
        if (regex.IsMatch(stageName))
            PlayerPrefs.SetString("LastStage", stageName);
    }
}
