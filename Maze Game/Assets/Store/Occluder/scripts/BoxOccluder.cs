using UnityEngine;
using System.Linq;

public class BoxOccluder : Occluder
{
    public Vector3 Size = Vector3.one;

    private Vector3[][] edges;

    private Vector3[][] GetRawEdges()
    {
        Vector3[][] edges = new Vector3[6][];

        // back
        edges[0] = new Vector3[4];
        edges[0][0] = new Vector3(-Size.x, -Size.y, -Size.z) * 0.5f;
        edges[0][1] = new Vector3(Size.x, -Size.y, -Size.z) * 0.5f;
        edges[0][2] = new Vector3(Size.x, Size.y, -Size.z) * 0.5f;
        edges[0][3] = new Vector3(-Size.x, Size.y, -Size.z) * 0.5f;

        // front
        edges[1] = new Vector3[4];
        edges[1][0] = new Vector3(-Size.x, -Size.y, Size.z) * 0.5f;
        edges[1][1] = new Vector3(Size.x, -Size.y, Size.z) * 0.5f;
        edges[1][2] = new Vector3(Size.x, Size.y, Size.z) * 0.5f;
        edges[1][3] = new Vector3(-Size.x, Size.y, Size.z) * 0.5f;

        // left
        edges[2] = new Vector3[4];
        edges[2][0] = new Vector3(-Size.x, -Size.y, -Size.z) * 0.5f;
        edges[2][1] = new Vector3(-Size.x, -Size.y, Size.z) * 0.5f;
        edges[2][2] = new Vector3(-Size.x, Size.y, Size.z) * 0.5f;
        edges[2][3] = new Vector3(-Size.x, Size.y, -Size.z) * 0.5f;

        // right
        edges[3] = new Vector3[4];
        edges[3][0] = new Vector3(Size.x, -Size.y, -Size.z) * 0.5f;
        edges[3][1] = new Vector3(Size.x, -Size.y, Size.z) * 0.5f;
        edges[3][2] = new Vector3(Size.x, Size.y, Size.z) * 0.5f;
        edges[3][3] = new Vector3(Size.x, Size.y, -Size.z) * 0.5f;

        // bottom
        edges[4] = new Vector3[4];
        edges[4][0] = new Vector3(Size.x, -Size.y, -Size.z) * 0.5f;
        edges[4][1] = new Vector3(-Size.x, -Size.y, -Size.z) * 0.5f;
        edges[4][2] = new Vector3(-Size.x, -Size.y, Size.z) * 0.5f;
        edges[4][3] = new Vector3(Size.x, -Size.y, Size.z) * 0.5f;

        // top
        edges[5] = new Vector3[4];
        edges[5][0] = new Vector3(Size.x, Size.y, -Size.z) * 0.5f;
        edges[5][1] = new Vector3(-Size.x, Size.y, -Size.z) * 0.5f;
        edges[5][2] = new Vector3(-Size.x, Size.y, Size.z) * 0.5f;
        edges[5][3] = new Vector3(Size.x, Size.y, Size.z) * 0.5f;

        return edges;
    }

	void OnDestroy()
	{
		Occluder.Occluders.Remove(this);
	}
	
    void Awake()
    {
		Occluder.Occluders.Add(this);
        edges = GetRawEdges();

        if (this.gameObject.isStatic)
            for (int i = 0; i < 6; ++i)
                edges[i] = OccluderUtility.CalculateWorldSpaceEdges(this.transform, edges[i]);
    }

    void OnDrawGizmos()
    {
        var worldSpaceEdges = this.ExtractWorldSpaceOccluderEdges();
        Gizmos.color = this.IsUsable ? Color.green : Color.red;
        for (int i = 0; i < 6; ++i)
        {
            Gizmos.DrawLine(worldSpaceEdges[i][0], worldSpaceEdges[i][1]);
            Gizmos.DrawLine(worldSpaceEdges[i][1], worldSpaceEdges[i][2]);
            Gizmos.DrawLine(worldSpaceEdges[i][2], worldSpaceEdges[i][3]);
            Gizmos.DrawLine(worldSpaceEdges[i][3], worldSpaceEdges[i][0]);
        }
    }

    public Vector3[][] ExtractWorldSpaceOccluderEdges()
    {
        if (!Application.isPlaying)
		{
			var tmpEdges = GetRawEdges();

			for (int i = 0; i < 6; ++i)
				tmpEdges[i] = OccluderUtility.CalculateWorldSpaceEdges(this.transform, tmpEdges[i]);

			return tmpEdges;
		}
		else
		{
			var occluderWorldSpaceEdges = new Vector3[6][];
			for(int i = 0; i < 6; ++i)
			{
				occluderWorldSpaceEdges[i] = gameObject.isStatic ? edges[i] : OccluderUtility.CalculateWorldSpaceEdges(this.transform, edges[i]);
			}
			return occluderWorldSpaceEdges;
		}
    }

    public override bool IsOccluding(Vector3[] otherWorldSpaceEdges)
    {
		var tmpEdges = ExtractWorldSpaceOccluderEdges();
        if (IsOccluding(otherWorldSpaceEdges, 0, -this.transform.forward, Size.z / 2, tmpEdges))
            return true;
        if (IsOccluding(otherWorldSpaceEdges, 1, this.transform.forward, Size.z / 2, tmpEdges))
            return true;
        if (IsOccluding(otherWorldSpaceEdges, 2, -this.transform.right, Size.x / 2, tmpEdges))
            return true;
        if (IsOccluding(otherWorldSpaceEdges, 3, this.transform.right, Size.x / 2, tmpEdges))
            return true;
        if (IsOccluding(otherWorldSpaceEdges, 4, -this.transform.up, Size.y / 2, tmpEdges))
            return true;
        if (IsOccluding(otherWorldSpaceEdges, 5, this.transform.up, Size.y / 2, tmpEdges))
            return true;
        return false;
    }

    private bool IsOccluding(Vector3[] otherWorldSpaceEdges, int index, Vector3 normal, float amount, Vector3[][] edges)
    {
        var occluderWorldSpaceEdges = edges[index];
        
        if (OccluderUtility.IsOccluding(normal, this.transform.position + normal * amount, occluderWorldSpaceEdges, otherWorldSpaceEdges))
            return true;
        return false;
    }

    public override bool IsVisible
    {
        get
        {
            var boxEdges = ExtractWorldSpaceOccluderEdges();
            var edges = boxEdges.SelectMany(e => e).Distinct().ToArray();
            return OccluderUtility.IsVisibleToCamera(edges);
        }
    }
}