using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractableObject : MonoBehaviour
{
    public GameObject interactButton; // Assign the button GameObject in the Inspector
    public Vector3 buttonOffset = new Vector3(1.0f, 0.5f, 0); // Adjust offset as needed
    private bool isPlayerInsideInteractArea = false;

    private GameObject player; // Reference to the player object
    private Canvas interactButtonCanvas; // Reference to the interactButton's canvas
    private Canvas floatingMessageCanvas; //Reference to the floatingMessage's canvas

    public GameObject messagePrefab; // Reference to the floating message
    public GameObject messageBoxPrefab;
    private float messageDuration = 5f; //How long the mssage will stay visible
    private float moveSpeed = 100f; // Speed of upward movement
    private float fadeSpeed = 0.5f; // Speed of fading out

    private bool messageRunning = false;

    public bool useMessageBox = false; // Toggle between floating message and message box
    public bool canMultipleInteract = true;
    private GameObject activeMessageBox;

    private InteractableButtonManager buttonManager;
    private Door doorManager;
    private Staircase staircaseManager;

    //For Custom Messages
    public TMP_InputField inputField; // Unique TextMeshPro InputField for this interactable
    public string customMessage;

    private void Start()
    {
        buttonManager = FindObjectOfType<InteractableButtonManager>();
        doorManager = FindObjectOfType<Door>();
        staircaseManager = FindObjectOfType<Staircase>();

        // Ensure the interactButton is initially inactive
        if (interactButton != null)
        {
            interactButton.SetActive(false);
            if (interactButton.GetComponent<CanvasGroup>() == null)
            {
                interactButton.AddComponent<CanvasGroup>().alpha = 0.5f; // Default alpha
            }
        }

        player = GameObject.FindGameObjectWithTag("Player");
        interactButtonCanvas = GameObject.Find("InteractableButtonCanvas").GetComponent<Canvas>();
        floatingMessageCanvas = GameObject.Find("FloatingMessageCanvas").GetComponent<Canvas>();

        if (inputField != null && string.IsNullOrEmpty(customMessage))
        {
            // Set default text only if customMessage is not already set
            inputField.text = "Default Message"; // Placeholder or empty string
        }

        // Listen for inputField changes
        if (inputField != null)
        {
            inputField.onEndEdit.AddListener(SetCustomMessage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!messageRunning && collider.CompareTag("Player") && gameObject.CompareTag("Interactable"))
        {
            // Position the button beside the interactable object
            if (interactButton != null)
            {
                interactButton.SetActive(true);

                /*
                // Convert the player's world position to screen space
                Vector3 screenPos = Camera.main.WorldToScreenPoint(player.transform.position + buttonOffset);

                // Convert screen space to local position relative to the interactButtonCanvas
                Vector3 localPos = interactButtonCanvas.transform.InverseTransformPoint(screenPos);

                // Set the interactButton position using RectTransform (UI elements)
                interactButton.GetComponent<RectTransform>().localPosition = localPos;*/

                buttonManager.AddInteractButton(interactButton);

                // Update the alpha values of all buttons
                buttonManager.UpdateButtonAlphas(); //On Trial
            }
            isPlayerInsideInteractArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && gameObject.CompareTag("Interactable"))
        {
            // Disable the interactButton
            if (interactButton != null)
            {
                interactButton.SetActive(false);
                buttonManager.RemoveInteractButton(interactButton);
                buttonManager.UpdateButtonAlphas();
            }
            isPlayerInsideInteractArea = false;

            if (activeMessageBox != null)
            {
                Destroy(activeMessageBox); // Close the message box if the player exits
            }
        }
    }

    private void Update()
    {
        // Check for interaction only if the player is inside the interactable area
        if (isPlayerInsideInteractArea && Input.GetKeyDown(KeyCode.E) && !messageRunning)
        {
            Debug.Log("Player interacted with: " + gameObject.name);
            PerformInteraction();
        }
    }
    public void PerformInteraction()
    {
        // Get the CanvasGroup of the interactButton
        CanvasGroup canvasGroup = interactButton.GetComponent<CanvasGroup>();

        // Only interact if the button's alpha is 1.0 (fully visible)
        if (canvasGroup != null && canvasGroup.alpha < 1.0f)
        {
            Debug.Log("Cannot interact: Button is not selected.");
            return;
        }

        // Check if this interactable is a door and call HeadOutside
        if (doorManager != null && gameObject.name.Contains("Door") && !Door.outside)
        {
            Debug.Log($"Interacting with door: {gameObject.name}");
            Door.outside = true;
            doorManager.HeadOutside();
            return; // No need to show messages for the door interaction
        }

        else if (doorManager != null && gameObject.name.Contains("Door") && Door.outside)
        {
            Debug.Log($"Interacting with door: {gameObject.name}");
            Door.outside = false;
            doorManager.HeadInside();
            return; // No need to show messages for the door interaction
        }

        if (staircaseManager != null && gameObject.name.Contains("Staircase") && !Staircase.down)
        {
            Debug.Log($"Interacting with staircase: {gameObject.name}");
            Staircase.down = true;
            staircaseManager.HeadDownstairs();
            return; // No need to show messages for the door interaction
        }

        if (staircaseManager != null && gameObject.name.Contains("Staircase") && Staircase.down)
        {
            Debug.Log($"Interacting with staircase: {gameObject.name}");
            Staircase.down = false;
            staircaseManager.HeadUpstairs();
            return; // No need to show messages for the door interaction
        }

        // Implement what happens when the player interacts
        Debug.Log($"Performing interaction with {gameObject.name}!");

        // Disable the interactButton while the message is being shown
        if (interactButton != null && !canMultipleInteract) interactButton.SetActive(false);

        if (useMessageBox)
        {
            ShowMessageBox(customMessage);
        }
        else
        {
            ShowFloatingMessage(customMessage);
        }
    }

    private void ShowMessageBox(string message)
    {
        if (activeMessageBox != null) return;

        if (messageBoxPrefab != null)
        {
            activeMessageBox = Instantiate(messageBoxPrefab, floatingMessageCanvas.transform);

            // Get the TextMeshProUGUI component from the message prefab
            TextMeshProUGUI messageText = activeMessageBox.GetComponentInChildren<TextMeshProUGUI>();

            if (messageText != null) messageText.text = message;


            /*
            // Position the message box above the interactable button
            RectTransform interactButtonRect = interactButton.GetComponent<RectTransform>();
            Vector3 buttonWorldPos = interactButtonRect.position; // World position of the button
            Vector3 buttonScreenPos = Camera.main.WorldToScreenPoint(buttonWorldPos); // Convert to screen space

            // Convert screen space to local space of the floatingMessageCanvas
            RectTransform canvasRect = floatingMessageCanvas.GetComponent<RectTransform>();
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, buttonScreenPos, Camera.main, out localPos);

            RectTransform messageBoxRect = activeMessageBox.GetComponent<RectTransform>();
            Vector2 messageBoxOffset = new Vector2(0, 100f); // Offset to place it above the button
            messageBoxRect.localPosition = localPos + messageBoxOffset;*/

            // Handle closing the message box and reactivating the interactable button
            Button closeButton = activeMessageBox.GetComponentInChildren<Button>();
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(() =>
                {
                    Destroy(activeMessageBox);
                    activeMessageBox = null;

                    // Reactivate the interactable button if still inside the trigger area
                    if (isPlayerInsideInteractArea && interactButton != null)
                    {
                        interactButton.SetActive(true);
                    }
                });
            }
        }
    }

    private void ShowFloatingMessage(string message)
    {
        if (messageRunning) return; // Prevent multiple messages from showing at once
        if (!canMultipleInteract) messageRunning = true;

        if (messagePrefab != null && interactButton != null)
        {
            // Instantiate the floating message prefab
            GameObject messageObject = Instantiate(messagePrefab, floatingMessageCanvas.transform);
            messageObject.gameObject.SetActive(true);

            // Get the TextMeshProUGUI component from the message prefab
            TextMeshProUGUI messageText = messageObject.GetComponent<TextMeshProUGUI>();

            if (messageText != null) messageText.text = message;

            // Add a CanvasGroup component to the message for fade effect (if not already added)
            CanvasGroup canvasGroup = messageObject.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = messageObject.AddComponent<CanvasGroup>();
            }

            // Convert interactButton's position to world space, then to screen space
            RectTransform interactButtonRect = interactButton.GetComponent<RectTransform>();
            Vector3 buttonWorldPos = interactButtonRect.position; // Get world position of the button
            Vector3 buttonScreenPos = Camera.main.WorldToScreenPoint(buttonWorldPos); // Convert to screen space

            // Convert screen space to local space of the floatingMessageCanvas
            RectTransform canvasRect = floatingMessageCanvas.GetComponent<RectTransform>();
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, buttonScreenPos, Camera.main, out localPos);

            // Set the position of the floating message
            RectTransform messageRect = messageObject.GetComponent<RectTransform>();
            Vector2 floatingMessageOffset = new Vector2(0, 100f); // Offset for the floating message
            messageRect.localPosition = localPos + floatingMessageOffset;

            // Start the animation for upward movement and fade-out
            StartCoroutine(AnimateFloatingMessage(messageObject, canvasGroup));
        }
    }


    private IEnumerator AnimateFloatingMessage(GameObject messageObject, CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;

        // Calculate start and target positions for the animation
        Vector3 startPosition = messageObject.GetComponent<RectTransform>().position;
        Vector3 targetPosition = startPosition + new Vector3(0, moveSpeed, 0); // Dynamically use moveSpeed

        // Animate for the duration of messageDuration
        while (elapsedTime < messageDuration)
        {
            elapsedTime += Time.deltaTime;

            // Smoothly move the message upwards
            messageObject.GetComponent<RectTransform>().position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / messageDuration);

            // Smoothly fade the message out using fadeSpeed
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / (messageDuration / fadeSpeed));

            yield return null;
        }

        // After the animation is complete, destroy the message object
        Destroy(messageObject);

        // Re-enable the interactButton if the player is still in the interaction area
        if (isPlayerInsideInteractArea && interactButton != null && !canMultipleInteract)
        {
            interactButton.SetActive(true);
        }

        if (!canMultipleInteract)
        {
            messageRunning = false;
        }
    }
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void SetMessageDuration(float duration)
    {
        messageDuration = duration;
    }

    //For System Managers
    public void SetCanMultipleInteract(bool canInteract)
    {
        canMultipleInteract = !canInteract;
    }

    public void SetUseMessageBox(bool use)
    {
        useMessageBox = use;
    }

    public void SetCustomMessage(string message)
    {
        customMessage = message; // Update the custom message
        if (inputField != null)
        {
            inputField.text = message; // Sync with the UI field
        }
    }
}