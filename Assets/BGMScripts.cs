using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMScripts : MonoBehaviour
{
    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }
}
