using Sirenix.OdinInspector;
using UnityEngine;

public class CombatCutscene : MonoBehaviour
{
    [Title("GameObject Reference")]
    [SerializeField] GameObject blackCanvas;
    void Start()
    {
        GoCombatCutscene();
    }
    void GoCombatCutscene()
    {
        blackCanvas.SetActive(true);
        blackCanvas.GetComponent<Animator>().Play("DarkCanvas(255 -> 0)");

        Invoke("GoCombat", blackCanvas.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

    }

    public void GoCombat()
    {
        GlobalEventSystem.SendStartCombat();
    }
}
