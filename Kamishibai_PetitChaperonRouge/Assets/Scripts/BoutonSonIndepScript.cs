using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoutonSonIndepScript : MonoBehaviour
{
    [HideInInspector] public AudioSource sonIndepAudioSource;
    [HideInInspector] public AudioClip sonIndepClip;
    private LivreManagement scriptLivreManagement;

    void Start()
    {
        sonIndepAudioSource = GameObject.Find("SliderSonIndependant").GetComponent<AudioSource>();
        scriptLivreManagement = GameObject.Find("Main Camera").GetComponent<LivreManagement>();
    }

    public void SonIndepPlaySound()
    {
        if (!scriptLivreManagement.isSonPause)
        {
            sonIndepAudioSource.PlayOneShot(sonIndepClip);
        }
    }
}
