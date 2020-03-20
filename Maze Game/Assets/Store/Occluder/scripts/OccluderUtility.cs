using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class OccluderUtility
{
    public static bool IsOccluding(Vector3 normal, Vector3 center, Vector3[] occluderWorldSpaceEdges, Vector3[] otherWorldSpaceEdges)
    {
        if (!IsVisibleToCamera(occluderWorldSpaceEdges))
            return false;

		var plane = new Plane(normal, center);
        if (!IsOccluderInFrontOfOther(plane, otherWorldSpaceEdges))
            return false;
			
        var isOccluding = CheckOcclusion(center, plane, occluderWorldSpaceEdges, otherWorldSpaceEdges);
        return isOccluding;
    }

    private static bool CheckOcclusion(Vector3 center, Plane plane, Vector3[] occluderWorldSpaceEdges, Vector3[] otherWorldSpaceEdges)
    {
		var planes = new List<Plane>();
		for (int i = 0; i < occluderWorldSpaceEdges.Length; ++i)
		{
			var p1 = (occluderWorldSpaceEdges[i]+occluderWorldSpaceEdges[(i+1) % occluderWorldSpaceEdges.Length]) / 2;
			var p1n = (center - p1).normalized;
			planes.Add(new Plane(p1n, p1));
		}
		
		var camPos = Camera.current.transform.position;
		foreach (var edge in otherWorldSpaceEdges)
		{
			var dirToCam = (camPos - edge).normalized;
			float distance;
			if (!plane.Raycast(new Ray(edge, dirToCam), out distance))
				return false;
			var posOnPlane = edge + dirToCam * distance;
			if (planes.Any(p => !p.GetSide(posOnPlane)))
				return false;
		}
        
		return true;
    }

    internal static bool IsVisibleToCamera(Vector3[] vertices)
    {
        var frustum = OcclusionCamera.CurrentCameraFrustum;
        for (int f = 0; f < 6; f++)
        {
            int p;
            for (p = 0; p < vertices.Length; p++)
            {
                if (frustum[f].GetSide(vertices[p]))
                    break;
            }
            if (p == vertices.Length)
                return false;
        }
        return true;
    }

    private static bool IsOccluderInFrontOfOther(Plane plane, Vector3[] otherWorldSpaceEdges)
    {
        var dirToCam = -Camera.current.transform.forward;
        return otherWorldSpaceEdges.All(e =>
        {
            float distance;
            return plane.Raycast(new Ray(e, dirToCam), out distance);
        });
    }

    public static Vector3[] CalculateWorldSpaceEdges(Transform otherTransform, Vector3[] otherEdges)
    {
        return otherEdges.Select(e => otherTransform.TransformPoint(e)).ToArray();
    }
}