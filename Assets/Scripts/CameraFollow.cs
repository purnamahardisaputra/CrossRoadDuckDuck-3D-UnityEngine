using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Vector3 offset;

    private void Start()
    {
        offset = this.transform.position - player.transform.position;
    }

    Vector3 lastBinatangPos;

    void Update()
    {
        if (player.IsDie || lastBinatangPos == player.transform.position)
            return;

        var targetBinatangPos = new Vector3(
            player.transform.position.x,
            0,
            player.transform.position.z
        );

        transform.position = targetBinatangPos + offset;
        lastBinatangPos = player.transform.position;
    }
}
