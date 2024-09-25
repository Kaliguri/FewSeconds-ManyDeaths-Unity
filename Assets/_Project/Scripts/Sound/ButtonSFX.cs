using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    private AudioSource AudioSource => GetComponent<AudioSource>();

    [SerializeField] private AudioClip EnterSFX;
    [SerializeField] private AudioClip ClickSFX;


        public void PlayEnterSFX()
    {
        AudioSource.PlayOneShot(EnterSFX);
    }
    public void PlayClickSFX()
    {
        AudioSource.PlayOneShot(ClickSFX);
    }

}
