using System;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EditorTranslator : EditorObject
{

    public override Editor GetEditorType()
    {
        return Editor.Translator;
    }

    public override void EditorMove(Vector3 delta) {
        Target.transform.SetLocalPositionAndRotation(transform.position + delta, transform.rotation);
        base.EditorMove(delta);
    }

    public override void SetPosition() {
        transform.SetPositionAndRotation(Target.transform.position + Target.transform.forward * -3 + Target.transform.right * -3, transform.rotation);
    }
}
