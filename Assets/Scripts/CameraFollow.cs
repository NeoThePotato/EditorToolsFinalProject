using UnityEngine;

[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject cameraTarget;

    private void OnValidate()
    {
        //There must be only 1 GO tagged "Player" in the scene for this to work
        if (cameraTarget == null)
        {
            cameraTarget = GameObject.FindGameObjectWithTag("Player");
            if (cameraTarget == null)
                Debug.Log("Player tag not found in scene");
        }
    }

    private void Update()
    {
        Vector3 targetPos = cameraTarget.transform.position;
        //transform.position = new Vector3(targetPos.x, targetPos.y + 0.6f, targetPos.z);

        //Third person camera attempt, did not work
        transform.position = targetPos + cameraTarget.transform.forward * -3f;
        transform.position = new Vector3(transform.position.x, targetPos.y + 1.7f, transform.position.z);
        transform.LookAt(targetPos + cameraTarget.transform.forward * 5f);
    }
}
