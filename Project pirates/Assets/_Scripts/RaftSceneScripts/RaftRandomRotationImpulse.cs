using UnityEngine;

public class RaftRandomRotationImpulse : MonoBehaviour
{
    private void OnEnable()
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.AddTorque(Random.insideUnitSphere / 2f, ForceMode.Impulse);
        }
        // disable fog
        RenderSettings.fog = false;
    }


}