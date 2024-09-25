using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sonity;

public class Play_menumusic : MonoBehaviour
{
    public SoundEvent menu;
    
    void Start()
    {
        menu.Play(transform);
    }

    
    void Update()
    {
        
    }
}
