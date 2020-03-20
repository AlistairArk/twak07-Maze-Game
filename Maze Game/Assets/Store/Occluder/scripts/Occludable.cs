using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Occludable : MonoBehaviour
{
	public static List<Occludable> Occludables = new List<Occludable>();
	
	private class RendererState
	{
		public bool WasEnabled;
	}
	
	public bool SendOcclusionMessages = false;
	public bool DisableRenderers = true;

    private bool isNewFrame = true;
	private Vector3[] staticEdges;
	private Dictionary<Renderer, RendererState> rendererStates = new Dictionary<Renderer, RendererState>();
	private bool wasEnabledLastFrame = true;

    void Start()
    {
		Occludables.Add(this);
		
		if (DisableRenderers)
		{
			var allRenderer = GetComponentsInChildren<Renderer>(true);
			foreach (var renderer in allRenderer)
				rendererStates.Add(renderer, new RendererState() { WasEnabled = renderer.enabled });

            if (allRenderer.Length == 0)
                Debug.LogWarning(name + " has not found any attached renderer on this or child elements.");
		}
		
		if (this.gameObject.isStatic)
			staticEdges = ExtractEdges();
    }
	
	void OnDestroy()
	{
		Occludables.Remove(this);
	}

    void LateUpdate()
    {
        if (!isNewFrame)
       		return;
        isNewFrame = false;
		
		if (DisableRenderers)
		{
			foreach (var pair in rendererStates)
				if (pair.Key)
					pair.Value.WasEnabled = pair.Key.enabled;
		}
    }
	
	public void OnBeginRender()
	{
		CullAgainstOccluders();
	}
	
	public void OnEndRender()
	{
		isNewFrame = true;
		
		if (DisableRenderers)
		{
			foreach (var pair in rendererStates)
				if (pair.Key)
					pair.Key.enabled = pair.Value.WasEnabled;
		}
	}	

    private void CullAgainstOccluders()
    {	
        var edges = staticEdges ?? ExtractEdges();

        foreach (var occluder in Occluder.Occluders)
        {
            if (!occluder.IsUsable)
                continue;

            if (occluder.gameObject == this.gameObject)
                continue;

            if (occluder.IsOccluding(edges))
            {
				if (wasEnabledLastFrame && SendOcclusionMessages)
					SendMessageUpwards("OnOccluderInvisible", this, SendMessageOptions.DontRequireReceiver);
				wasEnabledLastFrame = false;
				
				if (DisableRenderers)
				{
					foreach (var pair in rendererStates)
						if (pair.Key)
							pair.Key.enabled = false;
				}
				
                return;
            }
        }

        if (!wasEnabledLastFrame && SendOcclusionMessages)
			SendMessageUpwards("OnOccluderVisible", this, SendMessageOptions.DontRequireReceiver);
		wasEnabledLastFrame = true;
    }
	
	private Vector3[] ExtractEdges()
	{
		var totalBounds = GetComponentInChildren<Renderer>().bounds;
		var renderers = GetComponentsInChildren<Renderer>();
		foreach (var render in renderers)
			totalBounds.Encapsulate(render.bounds);
		var edges = ExtractBoundsEdges(totalBounds);
		
		return edges;
	}

    private Vector3[] ExtractBoundsEdges(Bounds bounds)
    {
        var center = bounds.center;
        var extents = bounds.extents;

        var newEdges = new Vector3[]
			{
				center + new Vector3(extents.x, extents.y, extents.z),
				center + new Vector3(extents.x, -extents.y, extents.z),
				center + new Vector3(-extents.x, extents.y, extents.z),
				center + new Vector3(-extents.x, -extents.y, extents.z),
				center + new Vector3(extents.x, extents.y, -extents.z),
				center + new Vector3(extents.x, -extents.y, -extents.z),
				center + new Vector3(-extents.x, extents.y, -extents.z),
				center + new Vector3(-extents.x, -extents.y, -extents.z)
			};
        return newEdges;
    }

    public bool IsVisble
    {
        get
        {
            var edges = staticEdges ?? ExtractEdges();
            return OccluderUtility.IsVisibleToCamera(edges);
        }
    }
}