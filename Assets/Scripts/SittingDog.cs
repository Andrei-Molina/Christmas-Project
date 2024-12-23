using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SittingDog : MonoBehaviour
{
    public AudioManager audioManager;
    public int randomNumber;

    private bool isPlayerInDogArea = false;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        if (!this.audioManager.IsSFXPlaying() && isPlayerInDogArea && Input.GetKeyDown(KeyCode.Alpha0))
        {
            randomNumber = UnityEngine.Random.Range(5, 8);
            audioManager.PlaySFX(randomNumber);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && gameObject.CompareTag("SittingDog"))
        {
            randomNumber = UnityEngine.Random.Range(5, 8);
            audioManager.PlaySFX(randomNumber);
            isPlayerInDogArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && gameObject.CompareTag("SittingDog"))
        {
            randomNumber = 0;
            audioManager.StopSFX();
            isPlayerInDogArea = false;
        }
    }
}
