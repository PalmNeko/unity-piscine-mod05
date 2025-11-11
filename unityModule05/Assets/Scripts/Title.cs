using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UIDocument))]
public class Title : MonoBehaviour
{
    private Button resumeButton;
    private Button diaryButton;
    private Button newGameButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        resumeButton = uiDocument.rootVisualElement.Q<Button>("Resume");
        diaryButton = uiDocument.rootVisualElement.Q<Button>("Diary");
        newGameButton = uiDocument.rootVisualElement.Q<Button>("NewGame");

        resumeButton.RegisterCallback<ClickEvent>(OnResume);
        diaryButton.RegisterCallback<ClickEvent>(OnDiary);
        newGameButton.RegisterCallback<ClickEvent>(OnNewGame);
    }

    void OnDisable()
    {
        resumeButton.UnregisterCallback<ClickEvent>(OnResume);
        diaryButton.UnregisterCallback<ClickEvent>(OnDiary);
        newGameButton.UnregisterCallback<ClickEvent>(OnNewGame);
    }

    void OnResume(ClickEvent evt)
    {
        if (PlayerPrefs.GetInt("HasSave") == 1)
            _ = GameManager.instance.LoadGame();
        else
            _ = GameManager.instance.NewGame();
    }

    void OnDiary(ClickEvent evt)
    {
        Debug.Log("OnDiary");
    }

    void OnNewGame(ClickEvent evt)
    {
        _ = GameManager.instance.NewGame();
    }
}
