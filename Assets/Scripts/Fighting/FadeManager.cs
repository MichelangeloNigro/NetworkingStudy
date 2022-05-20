using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable() {
        GetComponent<RawImage>().CrossFadeAlpha(0,0.1f,true);
        GetComponent<RawImage>().CrossFadeAlpha(255,10,true);
    }
}
