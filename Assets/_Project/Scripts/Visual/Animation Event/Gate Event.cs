using UnityEngine;
using UnityEngine.Events;

public class GateEvent : MonoBehaviour
{
    public static UnityEvent GateClosed = new();
    public void SendGateClosed() { GateClosed.Invoke(); }

    public static UnityEvent GateUp = new();
    public void SendGateUp() { GateUp.Invoke(); }

}
