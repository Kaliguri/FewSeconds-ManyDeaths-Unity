using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ColorBox : MonoBehaviour
{
    Color color => GetComponent<Image>().color;
    PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>().GetComponent<PlayerInfoData>();
    int playerID => playerInfoData.PlayerIDThisPlayer;
    public void ColorSelect ()
    {
        playerInfoData.ColorList[playerID] = color;
        GlobalEventSystem.SendPlayerDataChanged();
        GlobalEventSystem.SendPlayerColorChange();
    }
}
