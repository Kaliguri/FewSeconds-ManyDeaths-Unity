using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarColor : MonoBehaviour
{
    [SerializeField] private Image Fill;
    [SerializeField] private int UINumber;
    private PlayerInfoData Data => GameObject.FindObjectOfType<PlayerInfoData>();

    private void Start()
    {
        Fill.color = Data.ColorList[UINumber - 1];
    }
}
