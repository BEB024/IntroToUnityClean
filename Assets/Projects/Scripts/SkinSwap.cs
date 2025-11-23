using UnityEngine;
using UnityEngine.InputSystem;

public class SkinSwap : MonoBehaviour
{
    [Header("Player Materials (Up to 5)")]
    public Material[] materials;  // Assign materials in the Inspector
    private int currentMaterialIndex = 0;

    private Renderer bodyRenderer;

    void Start()
    {
        bodyRenderer = GetComponent<Renderer>();

        if (materials.Length > 0 && bodyRenderer != null)
        {
            ApplyMaterial(0);
        }
        else
        {
            Debug.LogWarning("SkinSwap: No materials assigned or no Renderer found on object.");
        }
    }

    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            CycleMaterial();
        }
    }

    void CycleMaterial()
    {
        if (materials.Length == 0) return;

        currentMaterialIndex = (currentMaterialIndex + 1) % materials.Length;
        ApplyMaterial(currentMaterialIndex);
    }

    void ApplyMaterial(int index)
    {
        if (bodyRenderer != null && materials[index] != null)
        {
            bodyRenderer.material = materials[index];
        }
    }
}
