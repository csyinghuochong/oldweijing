using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class ScreenCamera : MonoBehaviour
{
    [SerializeField] private float m_RenderTextureScale = 1;
    
    private Camera m_Camera;

    public Material Material;

    void Awake()
    {
        m_Camera = GetComponent<Camera>();
        UpdateTarget();
    }

    private void UpdateTarget()
    {
        //Shader.EnableKeyword("TEST_1");
        Material.EnableKeyword("TEST_1");
        m_Camera.targetTexture = new RenderTexture((int)(m_Camera.scaledPixelWidth * m_RenderTextureScale), (int)(m_Camera.scaledPixelHeight * m_RenderTextureScale), GraphicsFormat.R16G16B16A16_SFloat, GraphicsFormat.D24_UNorm_S8_UInt);
    }
}