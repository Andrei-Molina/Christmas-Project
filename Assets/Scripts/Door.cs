using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public GameObject player;
    public GameObject Inside;
    public GameObject Outside;
    public CameraFollow cameraFollow;
    public AudioManager audioManager;

    public static bool outside = false;

    public SnowflakeSpawner snowflakeSpawner;

    public void HeadOutside()
    {
        cameraFollow.minX = -1;
        cameraFollow.maxX = 1.8f;
        Inside.gameObject.SetActive(false);
        Outside.gameObject.SetActive(true);
        player.transform.position = new Vector3(-9.67f, -0.83f, 0);

        // Play the outside BGM (index 1)
        if (audioManager != null)
        {
            audioManager.PlayBGM(1);
        }

        snowflakeSpawner.DestroyAllSnowflakes();
    }

    public void HeadInside()
    {
        cameraFollow.minX = -1;
        cameraFollow.maxX = 9.4f;
        Inside.gameObject.SetActive(true);
        Outside.gameObject.SetActive(false);
        player.transform.position = new Vector3(14.80002f, -2.75f, 0);

        // Play the outside BGM (index 1)
        if (audioManager != null)
        {
            audioManager.PlayBGM(0);
        }

        snowflakeSpawner.DestroyAllSnowflakes();
    }
}
