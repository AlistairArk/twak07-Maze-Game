using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GuiScript : MonoBehaviour
{
    public OcclusionCamera cam;

    void OnGUI()
    {
        cam.enabled = GUILayout.Toggle(cam.enabled, "Enable Occlusion");
        cam.CullOccluders = GUILayout.Toggle(cam.CullOccluders, "Enable Occluder Culling");
    }
}