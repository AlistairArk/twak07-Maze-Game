using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Camera))]
public class OcclusionCamera : MonoBehaviour
{
    public bool CullOccluders = true;
    internal static Plane[] CurrentCameraFrustum;

    void OnPreCull() {
		if (!enabled)
			return;

        CurrentCameraFrustum = GeometryUtility.CalculateFrustumPlanes(Camera.current);

        foreach (var occluder in Occluder.Occluders)
            occluder.IsUsable = occluder.IsVisible;

        if (CullOccluders)
		    TagUsableOccluders();

        foreach (var occludable in Occludable.Occludables)
            if (occludable.IsVisble)
			    occludable.OnBeginRender();
	}
	
    
	void TagUsableOccluders() {
        foreach (var occluder in Occluder.Occluders)
		{
            if (occluder.IsUsable)
            {
                foreach (var otherOccludable in Occluder.Occluders)
                {
                    if (occluder != otherOccludable && otherOccludable.IsUsable)
                    {
                        if (otherOccludable.IsOccluding(occluder))
                        {
                            occluder.IsUsable = false;
                            break;
                        }
                    }
                }
            }
		}
	}
	

	void OnPostRender()	{
		if (!enabled)
			return;
		
		foreach (var occludable in Occludable.Occludables)
			occludable.OnEndRender();
	}
}