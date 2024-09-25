using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimationsFix : MonoBehaviour
{
    [SerializeField] private string NameStartAnimation = "Normal";
    public void FixAnimationForDisableObj()
    {
        Animator animation = GetComponent<Animator>();
        animation.CrossFade(NameStartAnimation, 0f);
        animation.Update(0f);
    }
}
