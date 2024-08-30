using System;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EditorRotator : EditorObject
{

    public override Editor GetEditorType()
    {
        return Editor.Rotator;
    }

    public override void EditorMove(Vector3 mousePos) {
        Vector3 p = Target.transform.position;
        Vector3 to_target =  new Vector3(mousePos.x, mousePos.y, 0f) - new Vector3(p.x, p.y, 0f);
        Vector3 f = Target.transform.forward;
        float angle = Vector3.Angle(new Vector3(f.x, f.y, 0f), to_target.normalized);
        Target.transform.Rotate(Vector3.up, angle);
        base.EditorMove(mousePos);
    }

    public override void SetPosition()
    {
        transform.SetPositionAndRotation(Target.transform.position + Target.transform.forward * 4, transform.rotation);
    }
}
