using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SittingCat : MonoBehaviour
{
    public AudioManager audioManager;
    public int randomNumber;
    public GameObject MeowBox;

    private bool isPlayerInCatArea = false;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();    
    }

    private void Update()
    {
        if (!this.audioManager.IsSFXPlaying() && isPlayerInCatArea && Input.GetKeyDown(KeyCode.Alpha0))
        {
            randomNumber = UnityEngine.Random.Range(0, 3);
            audioManager.PlaySFX(randomNumber);
        }

        // Toggle MeowBox based on whether the audio is playing
        if (this.audioManager.IsSFXPlaying())
        {
            MeowBox.SetActive(true);
        }
        else
        {
            MeowBox.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && gameObject.CompareTag("SittingCat"))
        {
            randomNumber = UnityEngine.Random.Range(0, 3);
            audioManager.PlaySFX(randomNumber);
            isPlayerInCatArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && gameObject.CompareTag("SittingCat"))
        {
            randomNumber = 0;
            audioManager.StopSFX();
            isPlayerInCatArea = false;
        }
    }
}
