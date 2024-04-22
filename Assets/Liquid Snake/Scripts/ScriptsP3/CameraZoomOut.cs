using UnityEngine;
using Cinemachine;

public class CameraZoomOut : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera_;
    [SerializeField]
    private float zoomSpeed_;
    [SerializeField]
    private float minFOV_;
    [SerializeField]
    private float maxFOV_;

    void Update()
    {
        Zoom();
    }

    void Zoom()
    {
        float scroll = -1.0f;
        if (Input.GetKey(KeyCode.Z)) // Si pulsamos la Z 
        {
            scroll = 1.0f; // Activamos el scroll
        }
        virtualCamera_.m_Lens.FieldOfView += scroll * zoomSpeed_ * Time.deltaTime;
        virtualCamera_.m_Lens.FieldOfView = Mathf.Clamp(virtualCamera_.m_Lens.FieldOfView, minFOV_, maxFOV_); // Clampeamos entre dos valores del FOV
    }
}