using UnityEngine;

[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject _cameraTarget;

    private void OnValidate()
    {
        //There must be only 1 GO tagged "Player" in the scene for this to work
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
        transform.position = new Vector3(_targetPos.x, _targetPos.y + 0.6f, _targetPos.z);

        //Third person camera attempt, did not work
        //Vector3 _targetPos = _cameraTarget.transform.position;
        //transform.position = _targetPos + _cameraTarget.transform.forward * -3f;
        //transform.position = new Vector3(transform.position.x, _targetPos.y + 1.7f, transform.position.z);
        //transform.LookAt(_targetPos + _cameraTarget.transform.forward * 5f);
    }
}
