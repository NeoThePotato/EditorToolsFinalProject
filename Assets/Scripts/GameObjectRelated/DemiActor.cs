using UnityEngine;

public class DemiActor : MonoBehaviour, IReseteable, IValidateable
{
    public bool IsValidated { get ; set; }

    public void Reset()
    {
        transform.position = Vector3.zero;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private void OnValidate()
    {
        ValidateScript();
    }

    public void ValidateScript()
    {
        IsValidated = true;
    }
}
