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
        if (GameManager.instance.CanBackTitle() == false)
            return;
        GameManager.instance.SaveData();
        gameObject.SetActive(false);
        GameManager.instance.PlayerVisible(false);
        ToTitle();
    }

    void ToTitle()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
