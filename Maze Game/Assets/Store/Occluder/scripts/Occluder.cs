using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class Occluder : MonoBehaviour
{
	public static List<Occluder> Occluders = new List<Occluder>();
	
	[HideInInspector]
	public bool IsUsable = true;
	
    public abstract bool IsOccluding(Vector3[] otherWorldSpaceEdges);

    public bool IsOccluding(Occluder other)
    {
        Vector3[] otherEgdes;
        if (other is PlaneOccluder)
        {
            otherEgdes = ((PlaneOccluder)other).ExtractWorldSpaceOccluderEdges();
        }
        else
        {
            var boxEdges = ((BoxOccluder)other).ExtractWorldSpaceOccluderEdges();
            otherEgdes = boxEdges.SelectMany(e => e).Distinct().ToArray();
        }
        return IsOccluding(otherEgdes);
    }

    public abstract bool IsVisible { get; }
}