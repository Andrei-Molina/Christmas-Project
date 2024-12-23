using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaTrigger : MonoBehaviour
{
    public AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && gameObject.CompareTag("Santa"))
        {
            audioManager.PlaySFX(3);
            audioManager.PlaySFX2(4);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && gameObject.CompareTag("Santa"))
        {
            audioManager.StopSFX();
            audioManager.StopSFX2();
        }
    }

}
