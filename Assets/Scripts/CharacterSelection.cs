using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Character
{
    public string name;
    public Sprite sprite;
    public RuntimeAnimatorController animatorController;

    // Animation state names
    public string idleAnimation;
    public string sidewalkAnimation;
}

public class CharacterSelection : MonoBehaviour
{
    public Button[] characterButtons;
    public SpriteRenderer characterRenderer;
    public Animator characterAnimator;
    public Character[] characters;
    public Character selectedCharacter;

    public void OnSelectCharacter(int index)
    {
        if (index >= 0 && index < characters.Length)
        {
            selectedCharacter = characters[index];

            if (characterRenderer != null)
            {
                characterRenderer.sprite = selectedCharacter.sprite;
            }

            if (characterAnimator != null && selectedCharacter.animatorController != null)
            {
                characterAnimator.runtimeAnimatorController = selectedCharacter.animatorController;
            }

            Debug.Log($"Selected character: {selectedCharacter.name}");
            ChangeActiveAlpha(index);
        }
    }

    private void ChangeActiveAlpha(int selectedIndex)
    {
        for (int i = 0; i < characterButtons.Length; i++)
        {
            Image buttonImage = characterButtons[i].GetComponent<Image>();
            if (buttonImage != null)
            {
                Color color = buttonImage.color;
                color.a = (i == selectedIndex) ? 1f : 0.5f; // Full alpha for selected, reduced for others
                buttonImage.color = color;
            }
        }
    }

    private void ResetAlphaToMax()
    {
        foreach (Button button in characterButtons)
        {
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
            {
                Color color = buttonImage.color;
                color.a = 1f;
                buttonImage.color = color;
            }
        }
    }

    public void ResetCharacterSelection()
    {
        selectedCharacter = null;

        // Reset the alpha of all character buttons to maximum
        ResetAlphaToMax();

        // Optionally reset the character renderer and animator
        if (characterRenderer != null)
        {
            characterRenderer.sprite = null;
        }

        if (characterAnimator != null)
        {
            characterAnimator.runtimeAnimatorController = null;
        }

        Debug.Log("Character selection has been reset.");
    }
}
