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
        if (SceneManager.GetActiveScene().name == "Boot")
            SceneManager.LoadScene("Title");
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

    public void PlayerVisible(bool visible)
    {
        player.gameObject.SetActive(visible);
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

    public void LoadGame()
    {
        PlayerVisible(true);
        LoadData();
        backTitleUI.gameObject.SetActive(true);
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
        await fader.FadeIn();
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
