using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.Collections;

public class CameraTransitionController : MonoBehaviour
{
    public enum TransitionType
    {
        FadeToBlack,
        ShrinkAndFade,
        SlideRight,
        SlideLeft,
        SlideUp,
        SlideDown
    }

    [System.Serializable]
    public class CameraTransitionSetup
    {
        public Button triggerButton;
        public CinemachineVirtualCamera targetCamera;
        public TransitionType transitionType;
    }

    public CameraTransitionSetup[] transitions;
    public float transitionDuration = 1f;
    public AnimationCurve easingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private CinemachineBrain cinemachineBrain;
    private Image fadeImage;
    private CinemachineVirtualCamera currentActiveCamera;

    private void Start()
    {
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();

        // Create a canvas for the fade effect
        Canvas canvas = new GameObject("FadeCanvas").AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;

        fadeImage = new GameObject("FadeImage").AddComponent<Image>();
        fadeImage.transform.SetParent(canvas.transform, false);
        fadeImage.color = Color.clear;
        fadeImage.rectTransform.anchorMin = Vector2.zero;
        fadeImage.rectTransform.anchorMax = Vector2.one;
        fadeImage.rectTransform.sizeDelta = Vector2.zero;

        // Set up button listeners
        foreach (var transition in transitions)
        {
            transition.triggerButton.onClick.AddListener(() => StartTransition(transition));
            transition.targetCamera.Priority = 0;  // Set all cameras to low priority initially
        }

        // Set the initial active camera
        if (transitions.Length > 0)
        {
            currentActiveCamera = transitions[0].targetCamera;
            currentActiveCamera.Priority = 10;
        }
    }

    private void StartTransition(CameraTransitionSetup transition)
    {
        StartCoroutine(PerformTransition(transition));
    }

    private IEnumerator PerformTransition(CameraTransitionSetup transition)
    {
        yield return StartCoroutine(TransitionOut(transition.transitionType));

        // Switch the active camera
        if (currentActiveCamera != null)
        {
            currentActiveCamera.Priority = 0;
        }
        transition.targetCamera.Priority = 10;
        currentActiveCamera = transition.targetCamera;

        yield return StartCoroutine(TransitionIn(transition.transitionType));
    }

    private IEnumerator TransitionOut(TransitionType type)
    {
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            float easedT = easingCurve.Evaluate(t);
            ApplyTransitionEffect(type, easedT, true);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator TransitionIn(TransitionType type)
    {
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            float easedT = easingCurve.Evaluate(1f - t);
            ApplyTransitionEffect(type, easedT, false);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void ApplyTransitionEffect(TransitionType type, float t, bool isTransitionOut)
    {
        switch (type)
        {
            case TransitionType.FadeToBlack:
                fadeImage.color = new Color(0, 0, 0, t);
                break;
            case TransitionType.ShrinkAndFade:
                fadeImage.color = new Color(0, 0, 0, t);
                break;
            case TransitionType.SlideRight:
            case TransitionType.SlideLeft:
            case TransitionType.SlideUp:
            case TransitionType.SlideDown:
                fadeImage.color = new Color(0, 0, 0, t);
                break;
        }
    }
}