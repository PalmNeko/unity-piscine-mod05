using UnityEngine;
using UnityEngine.UIElements;

public class Fader : MonoBehaviour
{
    public float fadeDuration = 1.0f;
    private VisualElement fade;

    void Start()
    {
        var document = GetComponent<UIDocument>();
        fade = document.rootVisualElement.Q<VisualElement>();
    }

    public async Awaitable FadeIn()
    {
        await Fade(1, 0, 1);
    }
    
    public async Awaitable FadeOut()
    {
        await Fade(0, 1, 1);
    }

    async Awaitable Fade(float from, float to, float duration)
    {
        var startFade = Time.time;
        var endFade = startFade + duration;

        Color newColor = Color.black;
        while (Time.time < endFade)
        {
            newColor.a = Mathf.Lerp(from, to, (Time.time - startFade) / duration);
            fade.style.backgroundColor = newColor;
            await Awaitable.NextFrameAsync();
        }
        newColor.a = to;
        fade.style.backgroundColor = newColor;
    }
}
