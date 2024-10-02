using SonityTemplate;
using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    private TemplateSoundPlayUI UITemplate => FindObjectOfType<TemplateSoundPlayUI>();

    public void PlayButtonHover()
    {
        UITemplate.PlayButtonHover();
    }
    public void PlayButtonClick()
    {
        UITemplate.PlayButtonClick();
    }

}
