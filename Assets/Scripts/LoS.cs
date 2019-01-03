using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoS : MonoBehaviour
{
    public static bool IsInLoS(GameObject obj1, GameObject obj2, float visionRange, float visionAngle, LayerMask mask)
    {
        if (Vector2.Distance(obj1.transform.position, obj2.transform.position) > visionRange)
            return false;

        Vector3 obj1Dir = obj1.transform.position - obj2.transform.position;
        float obj1Mgn = Vector3.Magnitude(obj1.transform.position);
        float thisMgn = Vector3.Magnitude(obj2.transform.position);

        float angle = Vector3.Dot(obj1.transform.position.normalized, obj2.transform.forward);

        if (angle < visionAngle)
            return false;

        RaycastHit hit;
        if (Physics.Raycast(obj2.transform.position, obj1Dir, out hit, visionRange, mask))
        {
            if (hit.transform.gameObject == obj1)
            {
                return true;
            }
        }

        return false;
    }

        public static bool IsInLoS(Vector3 objPos, GameObject viewer, float visionRange, float visionAngle)
    {
        if (Vector2.Distance(objPos, viewer.transform.position) > visionRange)
            return false;

        Vector3 obj1Dir = objPos - viewer.transform.position;
        float obj1Mgn = Vector3.Magnitude(objPos);
        float thisMgn = Vector3.Magnitude(viewer.transform.position);
        float dist = Vector3.Distance(objPos, viewer.transform.position);

        RaycastHit hit;
        if (Physics.Raycast(viewer.transform.position, obj1Dir, out hit, dist-.1f))
        {
            return false;
        }
        return false;
    }


}
