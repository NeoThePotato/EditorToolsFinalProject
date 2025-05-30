using UnityEngine;

[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject _cameraTarget;

    private void OnValidate()
    {
        if ( _cameraTarget == null)
        {
            _cameraTarget = GameObject.FindGameObjectWithTag("Player");
            if (_cameraTarget == null)
                Debug.Log("Player tag not found in scene");
        }
    }

    private void Update()
    {
        Vector3 _targetPos = _cameraTarget.transform.position;
        transform.position = _targetPos + _cameraTarget.transform.forward * -3f;
        transform.position = new Vector3(transform.position.x, _targetPos.y + 1.7f, transform.position.z);
        transform.LookAt(_targetPos + _cameraTarget.transform.forward * 5f);
    }
}
