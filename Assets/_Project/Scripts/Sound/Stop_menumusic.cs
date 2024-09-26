using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sonity;

public class Stop_menumusic : MonoBehaviour
{
    public SoundEvent menu;

    void Start()
    {
        menu.Stop(transform);
    }

    void Update()
    {
        
    }
}
