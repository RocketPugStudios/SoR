using UnityEditor;
using UnityEngine;
using static System.Net.WebRequestMethods;
using static Unity.Burst.Intrinsics.X86.Avx;
[CustomEditor(typeof(NPC_Behavior_Sight))]
public class FieldofViewEditor : Editor
{

    private void OnSceneGUI()
    {
        NPC_Behavior_Sight fov = (NPC_Behavior_Sight)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.sphere_radius);


        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.sphere_radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.sphere_radius);

        if (fov.canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.playerReference.transform.position);
        }

    }
    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}

