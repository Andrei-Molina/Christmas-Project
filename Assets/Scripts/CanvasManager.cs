using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject systemManager;
    public GameObject canvasSystemManager;
    public GameObject letterEditorCanvas;
    public GameObject letterEditorManager;
    public GameObject letterCanvas;

    public GameObject mainMenuCanvas;
    public GameObject characterSelectionCanvas;
    public GameObject gameplayScreen;
    public GameObject systemScreen;

    public CharacterSelection characterSelection;
    public CameraFollow cameraFollow;
    public CharacterController characterController;
    public SnowflakeSpawner snowflakeSpawner;

    public GameObject player;
    public GameObject insideEnvironment;
    public GameObject outsideEnvironment;

    private void Start()
    {
        characterSelection = FindObjectOfType<CharacterSelection>();
        cameraFollow = FindObjectOfType<CameraFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!canvasSystemManager.activeSelf)
            {
                canvasSystemManager.SetActive(true);
                systemManager.SetActive(true);
            }
            else
            {
                canvasSystemManager.SetActive(false);
                systemManager.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.F2) && !letterCanvas.activeSelf && !canvasSystemManager.activeSelf)
        {
            if (!letterEditorCanvas.activeSelf)
            {
                letterEditorCanvas.SetActive(true);
                letterEditorManager.SetActive(true);
            }
            else
            {
                letterEditorCanvas.SetActive(false);
                letterEditorManager.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.M) && !letterEditorCanvas.activeSelf && !canvasSystemManager.activeSelf)
        {
            if (!letterCanvas.activeSelf)
            {
                letterCanvas.SetActive(true);
            }
            else
            {
                letterCanvas.SetActive(false);
            }
        }
    }

    public void Gameplay()
    {
        if (characterSelection.selectedCharacter != null)
        {
            Debug.Log($"Proceeding with character: {characterSelection.selectedCharacter.name}");
            ClearUI();
            gameplayScreen.gameObject.SetActive(true);
            player.transform.position = new Vector3(0.64f, -2.75f, 0);
            Door.outside = false;
            Staircase.down = false;
            insideEnvironment.gameObject.SetActive(true);
            outsideEnvironment.gameObject.SetActive(false);
            cameraFollow.minX = -1;
            cameraFollow.maxX = 9.4f;

            characterController.UpdateCharacterAnimations(characterSelection.selectedCharacter);
            snowflakeSpawner.DestroyAllSnowflakes();
        }
        else
        {
            Debug.Log("No character selected.");
        }
    }

    public void MainMenu()
    {
        ClearUI();
        mainMenuCanvas.gameObject.SetActive(true);
        characterSelection.ResetCharacterSelection();
    }

    public void CharacterSelection()
    {
        ClearUI();
        characterSelectionCanvas.gameObject.SetActive(true);
    }

    private void ClearUI()
    {
        mainMenuCanvas.gameObject.SetActive(false);
        gameplayScreen.gameObject.SetActive(false);
        characterSelectionCanvas.gameObject.SetActive(false);
        systemScreen.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Game is closed");
        Application.Quit();
    }
}
