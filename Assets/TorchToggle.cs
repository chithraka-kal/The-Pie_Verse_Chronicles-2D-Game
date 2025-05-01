using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchToggle : MonoBehaviour
{
    public Light2D torchLight;

    void Update()
    {
        onTorchOn();
    }

    private void onTorchOn()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            torchLight.enabled = !torchLight.enabled;
        }
    }
}