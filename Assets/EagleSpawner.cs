using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class EagleSpawner : MonoBehaviour
{
    [SerializeField] GameObject eaglePrefab;
    [SerializeField] int spawnZPos = 7;
    [SerializeField] Player player;
    [SerializeField] float timeOut = 5;

    float timer = 0;
    int playerLastMaxTravel = 0;


    private void SpawnEagle()
    {
        player.enabled = false;
        var position = new Vector3(player.transform.position.x, 1, player.CurrentTravel + spawnZPos);
        var rotation = Quaternion.Euler(0, 180, 0);
        var eagleObject = Instantiate(eaglePrefab, position, rotation);
        var eagle = eagleObject.GetComponent<Eagle>();
        eagle.setUpTarget(player);

    }

    // IEnumerator void Timer()
    // {
    //     while (true)
    //     {

    //         yield return new WaitForEndOfFrame();
    //     }
    // }

    private void Update()
    {
        // jika player ada kemajuan
        if (player.MaxTravel != playerLastMaxTravel)
        {
            // maka reset timer
            timer = 0;
            playerLastMaxTravel = player.MaxTravel;
            return;
        }
        // kalau ngga maju2 jalankan timer
        if (timer < timeOut)
        {
            timer += Time.deltaTime;
            return;
        }

        // kalau sudah timeout
        if (player.IsJumping() == false && player.IsDie == false)
            SpawnEagle();

    }
}
