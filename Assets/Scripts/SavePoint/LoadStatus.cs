using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadStatus : MonoBehaviour
{
    public static bool LoadGame;

    private void Awake() {
        DontDestroyOnLoad(this);
    }
}
