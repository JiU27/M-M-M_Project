using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIButtonControl : MonoBehaviour
{
    [System.Serializable]
    public class ButtonGroup
    {
        public string groupName;
        public List<Button> buttonsToShow = new List<Button>();
        public List<Button> buttonsToHide = new List<Button>();
    }

    public List<Button> initialVisibleButtons = new List<Button>();
    public List<ButtonGroup> buttonGroups = new List<ButtonGroup>();

    private void Start()
    {
        // Set initial button states
        SetButtonsOrder(initialVisibleButtons, 2);
        foreach (var group in buttonGroups)
        {
            SetButtonsOrder(group.buttonsToHide, 0);
        }
    }

    private void OnEnable()
    {
        // Add listeners to all buttons
        foreach (var group in buttonGroups)
        {
            foreach (var button in group.buttonsToShow)
            {
                button.onClick.AddListener(() => OnButtonClicked(group));
            }
        }
    }

    private void OnDisable()
    {
        // Remove listeners from all buttons
        foreach (var group in buttonGroups)
        {
            foreach (var button in group.buttonsToShow)
            {
                button.onClick.RemoveListener(() => OnButtonClicked(group));
            }
        }
    }

    private void OnButtonClicked(ButtonGroup clickedGroup)
    {
        // Hide all buttons
        foreach (var group in buttonGroups)
        {
            SetButtonsOrder(group.buttonsToShow, 0);
        }

        // Show buttons for the clicked group
        SetButtonsOrder(clickedGroup.buttonsToShow, 2);
        SetButtonsOrder(clickedGroup.buttonsToHide, 0);
    }

    private void SetButtonsOrder(List<Button> buttons, int order)
    {
        foreach (var button in buttons)
        {
            Canvas canvas = button.GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = button.gameObject.AddComponent<Canvas>();
            }
            canvas.overrideSorting = true;
            canvas.sortingOrder = order;
        }
    }
}