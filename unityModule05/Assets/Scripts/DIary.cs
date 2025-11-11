using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class DIary : MonoBehaviour
{
    Label totalItemCount;
    Label totalDeathCount;
    Label stage1Locked;
    Label stage2Locked;
    Label stage3Locked;

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        totalItemCount = uiDocument.rootVisualElement.Q<Label>("TotalItemCount");
        totalDeathCount = uiDocument.rootVisualElement.Q<Label>("TotalDeathCount");
        stage1Locked = uiDocument.rootVisualElement.Q<Label>("Stage1Locked");
        stage2Locked = uiDocument.rootVisualElement.Q<Label>("Stage2Locked");
        stage3Locked = uiDocument.rootVisualElement.Q<Label>("Stage3Locked");
    }
    
    // Update is called once per frame
    void Update()
    {
        totalItemCount.text = GameManager.instance.totalCount.ToString();
        totalDeathCount.text = GameManager.instance.defeatCount.ToString();

        stage1Locked.text = "Lock";
        stage3Locked.text = "Lock";
        stage2Locked.text = "Lock";
        switch (GameManager.instance.lastStage)
        {
            case "Stage3":
                stage3Locked.text = "Unlocked";
                goto case "Stage2";
            case "Stage2":
                stage2Locked.text = "Unlocked";
                goto case "Stage1";
            case "Stage1":
                stage1Locked.text = "Unlocked";
                break;
        }
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
