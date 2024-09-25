using UnityEngine;
using UnityEngine.UIElements;

public class DynamicSpriteOrder : MonoBehaviour
{
    public float Devider = 10000;
    private Vector3 position => GetComponent<Transform>().position;


    // Update is called once per frame
    void LateUpdate()
    {
        GetComponent<Transform>().position = new Vector3(position.x, position.y, position.z + position.y/Devider);
    }
}
