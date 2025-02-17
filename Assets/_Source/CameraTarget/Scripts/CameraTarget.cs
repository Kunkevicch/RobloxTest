using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Transform _playerTargetFollower;
    [SerializeField] private bool _isRMBOnly;
    [SerializeField] private float _sensitivity;

    private float _rotationX = 0.0f;
    private float _rotationY = 0.0f;

    private void LateUpdate()
    {
        transform.position = _playerTargetFollower.position;

        if (_isRMBOnly)
        {
            if (Input.GetMouseButton(1))
            {
                HandleMouseRotation();
            }
        }
        else
        {
            HandleMouseRotation();
        }
    }

    private void HandleMouseRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * _sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivity;

        _rotationX -= mouseY;
        _rotationY += mouseX;

        _rotationX = Mathf.Clamp(_rotationX, -90, 90);

        transform.localRotation = Quaternion.Euler(_rotationX, _rotationY, 0);
    }
}
