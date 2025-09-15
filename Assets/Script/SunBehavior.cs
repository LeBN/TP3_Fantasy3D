using UnityEngine;

public class SunBehavior : MonoBehaviour
{
    [SerializeField] private float dayDuration = 24 * 60 * 60;
    float ROT_COEFF = 360;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(Vector3.right * ROT_COEFF * Time.deltaTime / dayDuration);
    }
}
