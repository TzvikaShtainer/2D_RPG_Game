using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private float flashDuration = 0.2f;
    [SerializeField] private Material hitMat;
    private Material originalMat;

    
    [Header("Elements Colors")]
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] shockColor;
    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;  
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        sr.material = originalMat;
        sr.color = currentColor;
    }

    private void RedColorBlink()
    {
        if(sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
    }
    
    
    //make this generic
    public void IgniteFxFor(float seconds)
    {
        InvokeRepeating("IgniteColorFx", 0, 0.3f);
        Invoke("CancelColorChange", seconds);
    }
    
    public void ChillFxFor(float seconds)
    {
        InvokeRepeating("ChillColorFx", 0, 0.3f);
        Invoke("CancelColorChange", seconds);
    }
    
    public void ShockFxFor(float seconds)
    {
        InvokeRepeating("ShockColorFx", 0, 0.3f);
        Invoke("CancelColorChange", seconds);
    }
    
    private void IgniteColorFx()
    {
        if(sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }
    
    private void ChillColorFx()
    {
        if(sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }
    
    private void ShockColorFx()
    {
        if(sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }
    
}
