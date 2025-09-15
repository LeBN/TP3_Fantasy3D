using UnityEngine;

public class AggroTrigger : MonoBehaviour
{
    private MonsterAI ai;

    void Awake() => ai = GetComponentInParent<MonsterAI>();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) ai?.SetAggro(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) ai?.SetAggro(false);
    }
}
