using UnityEngine;
using System.Linq;

public class PlaneOccluder : Occluder
{
    public Vector2 Size = Vector2.one;

    private Vector3[] edges;

    private Vector3[] GetRawEdges()
    {
        Vector3[] edges = new Vector3[4];

        edges[0] = new Vector3(-Size.x, -Size.y, 0) * 0.5f;
        edges[1] = new Vector3(Size.x, -Size.y, 0) * 0.5f;
        edges[2] = new Vector3(Size.x, Size.y, 0) * 0.5f;
        edges[3] = new Vector3(-Size.x, Size.y, 0) * 0.5f;

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
            edges = OccluderUtility.CalculateWorldSpaceEdges(this.transform, edges);
    }

    void OnDrawGizmos()
    {
        var worldSpaceEdges = this.ExtractWorldSpaceOccluderEdges();

        Gizmos.color = this.IsUsable ? Color.green : Color.red;
        Gizmos.DrawLine(worldSpaceEdges[0], worldSpaceEdges[1]);
        Gizmos.DrawLine(worldSpaceEdges[1], worldSpaceEdges[2]);
        Gizmos.DrawLine(worldSpaceEdges[2], worldSpaceEdges[3]);
        Gizmos.DrawLine(worldSpaceEdges[3], worldSpaceEdges[0]);
    }

    public Vector3[] ExtractWorldSpaceOccluderEdges()
    {
        if (!Application.isPlaying)
        {
            var tmpEdges = GetRawEdges();

            var occluderWorldSpaceEdges = OccluderUtility.CalculateWorldSpaceEdges(this.transform, tmpEdges);
            return occluderWorldSpaceEdges;
        }
        else
        {
            var occluderWorldSpaceEdges = gameObject.isStatic ? edges : OccluderUtility.CalculateWorldSpaceEdges(this.transform, edges);
            return occluderWorldSpaceEdges;
        }
    }

    public override bool IsOccluding(Vector3[] otherWorldSpaceEdges)
    {
        var occluderWorldSpaceEdges = ExtractWorldSpaceOccluderEdges();
        return OccluderUtility.IsOccluding(this.transform.forward, this.transform.position, occluderWorldSpaceEdges, otherWorldSpaceEdges);
    }

    public override bool IsVisible
    {
        get
        {
            var edges = ExtractWorldSpaceOccluderEdges();
            return OccluderUtility.IsVisibleToCamera(edges);
        }
    }
}