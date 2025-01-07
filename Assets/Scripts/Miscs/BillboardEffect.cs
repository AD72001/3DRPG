using UnityEngine;

public class BillboardEffect : MonoBehaviour
{
    public Camera _camera;

    private void Awake() {
        _camera = Camera.main;
    }

    private void LateUpdate() {
        if (!_camera)
            _camera = Camera.main;
            
        transform.forward = _camera.transform.forward;
    }
}
