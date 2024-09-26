using Sonity;
using UnityEngine;


public class SoundManagerReference : MonoBehaviour
{
    public SoundManager ManagerReference => GetComponent<SoundManager>();
}
