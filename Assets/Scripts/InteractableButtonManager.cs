using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableButtonManager : MonoBehaviour
{
    private List<GameObject> interactButtons = new List<GameObject>(); // Active buttons
    private int currentIndex = 0; // Index of the currently selected button
    public KeyCode interactKey = KeyCode.E; // Key to interact
    public KeyCode navigateUpKey = KeyCode.UpArrow; // Key to navigate up
    public KeyCode navigateDownKey = KeyCode.DownArrow; // Key to navigate down

    private void Update()
    {
        if (interactButtons.Count == 0) return;

        // Navigate through buttons
        if (Input.GetKeyDown(navigateDownKey))
        {
            currentIndex = (currentIndex - 1 + interactButtons.Count) % interactButtons.Count;
            UpdateButtonSelection();
        }
        else if (Input.GetKeyDown(navigateUpKey))
        {
            currentIndex = (currentIndex + 1) % interactButtons.Count;
            UpdateButtonSelection();
        }

        // Interact with the currently selected button
        if (Input.GetKeyDown(interactKey))
        {
            InteractableObject interactable = interactButtons[currentIndex].GetComponent<InteractableObject>();
            if (interactable != null) interactable.PerformInteraction();
        }
    }

    private void UpdateButtonSelection()
    {
        for (int i = 0; i < interactButtons.Count; i++)
        {
            CanvasGroup canvasGroup = interactButtons[i].GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = (i == currentIndex) ? 1.0f : 0.5f; // Highlight selected button
            }
        }
    }

    //Trial

    public void UpdateButtonAlphas()
    {
        // Ensure all buttons have alpha set to 128
        foreach (GameObject button in interactButtons)
        {
            CanvasGroup canvasGroup = button.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0.5f; // Set all buttons to alpha 128 (semi-transparent)
            }
        }

        // Set the alpha of the first button to 1.0 (fully visible)
        if (interactButtons.Count > 0)
        {
            CanvasGroup firstButtonCanvasGroup = interactButtons[0].GetComponent<CanvasGroup>();
            if (firstButtonCanvasGroup != null)
            {
                firstButtonCanvasGroup.alpha = 1.0f; // Set the first button to alpha 255 (fully visible)
            }
        }
    }


    public void AddInteractButton(GameObject button)
    {
        if (!interactButtons.Contains(button))
        {
            interactButtons.Add(button);
            UpdateButtonSelection(); // Update selection when a new button is added
        }
    }

    public void RemoveInteractButton(GameObject button)
    {
        if (interactButtons.Contains(button))
        {
            interactButtons.Remove(button);
            currentIndex = Mathf.Clamp(currentIndex, 0, interactButtons.Count - 1); // Adjust index
            UpdateButtonSelection();
        }
    }
}
