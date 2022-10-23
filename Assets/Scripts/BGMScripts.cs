using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMScripts : MonoBehaviour
{
    private void Awake() {
        if(FindObjectsOfType<BGMScripts>().Length > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }

    }
}
