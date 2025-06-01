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

    private void FixedUpdate()
    {
        Vector3 targetPos = cameraTarget.transform.position;

        transform.position = new Vector3(targetPos.x, targetPos.y + 0.6f, targetPos.z);
    }
}
