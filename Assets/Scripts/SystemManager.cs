using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public class SystemManager : MonoBehaviour
{
    // Boolean to track whether interaction should be disabled when the message is shown
    public bool disableMultipleInteraction = false;
    public bool useMessageBox = false;

    // List of all interactable objects in the game
    public List<InteractableObject> interactableObjects = new List<InteractableObject>();

    public Image imageDisableMultipleInteraction;
    public Image imageUseMessageBox;
    public Sprite[] cBoxSprites;

    //For Custom messages
    public InputField messageInputField; // Reference to the input field for custom messages
    public TMP_InputField fadeSpeedInputField; // InputField for fade speed
    public TMP_InputField moveSpeedInputField; // InputField for move speed
    private Dictionary<string, string> interactableMessages = new Dictionary<string, string>();

    private float messageDuration = 1.0f; // Default fade speed
    private float moveSpeed = 1.0f; // Default lerp speed

    private void Awake()
    {
        // Update message based on saved preferences, if any
        LoadPreferences();
        LoadMessages();
    }

    private void Start()
    {
        // Load saved value from PlayerPrefs
        if (PlayerPrefs.HasKey("DisableMultipleInteraction"))
        {
            disableMultipleInteraction = PlayerPrefs.GetInt("DisableMultipleInteraction") == 1;
        }

        // Load saved value from PlayerPrefs
        if (PlayerPrefs.HasKey("UseMessageBox"))
        {
            useMessageBox = PlayerPrefs.GetInt("UseMessageBox") == 1;
        }

        // Set the sprite based on the loaded value
        UpdateCheckboxVisual();

        // Apply the loaded state to all interactable objects
        foreach (var interactable in interactableObjects)
        {
            interactable.SetCanMultipleInteract(disableMultipleInteraction);
            interactable.SetUseMessageBox(useMessageBox);

            // Update interactable objects with speed preferences
            interactable.SetMessageDuration(messageDuration);
            interactable.SetMoveSpeed(moveSpeed);

            // Ensure interactable messages are correctly initialized
            if (!interactableMessages.ContainsKey(interactable.name))
            {
                interactableMessages[interactable.name] = "Default Message"; // Default value
            }

            // Apply loaded messages
            if (interactableMessages.TryGetValue(interactable.name, out var message))
            {
                interactable.SetCustomMessage(message);
            }
        }
    }

    public void UpdateMessageDuration()
    {
        if (float.TryParse(fadeSpeedInputField.text, out var newFadeSpeed))
        {
            messageDuration = Mathf.Clamp(newFadeSpeed, 0.1f, 500f); // Clamping to prevent extreme values
            foreach (var interactable in interactableObjects)
            {
                interactable.SetMessageDuration(messageDuration);
            }
            SavePreferences();
        }
    }

    public void UpdateMoveSpeed()
    {
        if (float.TryParse(moveSpeedInputField.text, out var newMoveSpeed))
        {
            moveSpeed = Mathf.Clamp(newMoveSpeed, 10f, 500f); // Clamping for reasonable movement speeds
            foreach (var interactable in interactableObjects)
            {
                interactable.SetMoveSpeed(moveSpeed);
            }
            SavePreferences();
        }
    }

    private void LoadPreferences()
    {
        messageDuration = PlayerPrefs.GetFloat("FadeSpeed", 1.0f); // Default to 1.0 if not set
        moveSpeed = PlayerPrefs.GetFloat("LerpSpeed", 1.0f); // Default to 1.0 if not set

        // Set the InputField text to reflect the loaded values
        if (fadeSpeedInputField != null)
        {
            fadeSpeedInputField.text = messageDuration.ToString("F2"); // Format to two decimal places
        }

        if (moveSpeedInputField != null)
        {
            moveSpeedInputField.text = moveSpeed.ToString("F2"); // Format to two decimal places
        }
    }

    public float GetFadeSpeed()
    {
        return messageDuration;
    }

    public float GetLerpSpeed()
    {
        return moveSpeed;
    }

    private void SavePreferences()
    {
        PlayerPrefs.SetFloat("FadeSpeed", messageDuration);
        PlayerPrefs.SetFloat("LerpSpeed", moveSpeed);
        PlayerPrefs.Save();
    }

    //For toggling buttons
    public void ToggleMultipleInteraction()
    {
        disableMultipleInteraction = !disableMultipleInteraction;

        if (disableMultipleInteraction == false)
        {
            imageDisableMultipleInteraction.sprite = cBoxSprites[0];
        }

        else
        {
            imageDisableMultipleInteraction.sprite = cBoxSprites[1];
        }

        foreach (var interactable in interactableObjects)
        {
            interactable.SetCanMultipleInteract(disableMultipleInteraction);
        }
    }

    public void ToggleUseMessageBox()
    {
        useMessageBox = !useMessageBox;
        if (useMessageBox == false)
        {
            imageUseMessageBox.sprite = cBoxSprites[0];
        }
        else
        {
            imageUseMessageBox.sprite = cBoxSprites[1];
        }

        foreach (var interactable in interactableObjects)
        {
            interactable.SetUseMessageBox(useMessageBox);
        }
    }

    //For updating visuals
    private void UpdateCheckboxVisual()
    {
        if (disableMultipleInteraction)
        {
            imageDisableMultipleInteraction.sprite = cBoxSprites[1]; // Checked sprite
        }
        else
        {
            imageDisableMultipleInteraction.sprite = cBoxSprites[0]; // Unchecked sprite
        }

        if (useMessageBox)
        {
            imageUseMessageBox.sprite = cBoxSprites[1];
        }
        else
        {
            imageUseMessageBox.sprite = cBoxSprites[0];
        }
    }

    //For saving PlayerPrefs

    private void OnApplicationQuit()
    {
        SaveState();
    }

    private void OnDestroy()
    {
        SaveState();
    }

    private void SaveState()
    {
        // Save the state to PlayerPrefs
        PlayerPrefs.SetInt("DisableMultipleInteraction", disableMultipleInteraction ? 1 : 0);
        PlayerPrefs.SetInt("UseMessageBox", useMessageBox ? 1 : 0);
        PlayerPrefs.Save(); // Ensure the data is written to disk
    }
    public void SaveMessages()
    {
        foreach (var interactable in interactableObjects)
        {
            string messageKey = $"Message_{interactable.name}";
            PlayerPrefs.SetString(messageKey, interactable.customMessage);
        }
        PlayerPrefs.Save();
    }

    public void DeleteAllSaved()
    {
        PlayerPrefs.DeleteAll();
    }
    public void LoadMessages()
    {
        foreach (var interactable in interactableObjects)
        {
            string messageKey = $"Message_{interactable.name}";
            if (PlayerPrefs.HasKey(messageKey))
            {
                string savedMessage = PlayerPrefs.GetString(messageKey);
                interactableMessages[interactable.name] = savedMessage;

                // Use SetCustomMessage to ensure both customMessage and InputField.text are synced
                interactable.SetCustomMessage(savedMessage);
            }
        }
    }
}
