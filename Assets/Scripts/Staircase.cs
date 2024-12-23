using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staircase : MonoBehaviour
{
    public GameObject player;
    public CameraFollow cameraFollow;

    public static bool down = false;

    public void HeadDownstairs()
    {
        cameraFollow.minX = -1;
        cameraFollow.maxX = 1.8f;
        down = true;
        player.transform.position = new Vector3(-2.77f, -2.75f, 0);
    }

    public void HeadUpstairs()
    {
        cameraFollow.minX = -1;
        cameraFollow.maxX = 9.4f;
        down = false;
        player.transform.position = new Vector3(-9.67f, -0.83f, 0);
    }
}
