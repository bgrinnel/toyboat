using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EditorObject : MonoBehaviour
{
    public enum Editor {
        Base,
        Translator,
        Rotator
    }
    protected GameObject Target;
    public virtual Editor GetEditorType() 
    {
        return Editor.Base;
    }
    public virtual void EditorMove(Vector3 vector3) {
        SetPosition();
    }

    public virtual void SetPosition() {}
}
