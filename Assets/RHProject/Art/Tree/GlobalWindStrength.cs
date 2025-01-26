using UnityEngine;

public class GlobalWindStrength : MonoBehaviour
{
    public float WindStrength = 1.0f;

    private void Update()
    {
        Shader.SetGlobalFloat("_WindStrength", WindStrength);
    }
}
