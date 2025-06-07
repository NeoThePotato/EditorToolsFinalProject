using UnityEngine;

public interface IValidateable
{

    public bool IsValidated { get; set; }
    public void ValidateScript();
   
}
