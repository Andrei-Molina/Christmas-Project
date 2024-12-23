using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private void Start()
    {
    }

    private void Update()
    {
        float x = Mathf.Clamp(player.transform.position.x, minX, maxX);
        float y = Mathf.Clamp(player.transform.position.y, minY, maxY);
        gameObject.transform.position = new Vector3(x, y, gameObject.transform.position.z);
    }
}
