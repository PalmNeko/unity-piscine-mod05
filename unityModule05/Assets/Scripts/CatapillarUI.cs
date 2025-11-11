using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class CatapillarUI : MonoBehaviour
{
    Label HPLabel;
    Label ItemCountLabel;

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        HPLabel = uiDocument.rootVisualElement.Q<Label>("HP");
        ItemCountLabel = uiDocument.rootVisualElement.Q<Label>("ItemCount");
    }

    void Update()
    {
        HPLabel.text = GameManager.instance.player.hp.ToString();
        ItemCountLabel.text = GameManager.instance.itemCount.ToString();
    }
}
