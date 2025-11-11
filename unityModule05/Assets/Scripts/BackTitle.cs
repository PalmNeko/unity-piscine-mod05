using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UIDocument))]
public class BackTitle : MonoBehaviour
{
    private Button backButton;

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        backButton = uiDocument.rootVisualElement.Q<Button>("BackTitle");

        backButton.RegisterCallback<ClickEvent>(OnBack);
    }

    void OnDisable()
    {
        backButton.UnregisterCallback<ClickEvent>(OnBack);
    }

    void OnBack(ClickEvent evt)
    {
        GameManager.instance.SaveData();
        ToTitle();
        gameObject.SetActive(false);
        GameManager.instance.PlayerVisible(false);
    }

    void ToTitle()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
