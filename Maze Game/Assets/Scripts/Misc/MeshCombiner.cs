// using UnityEngine;
// using System.Collections;

// public class MeshCombiner : MonoBehaviour {
//     public void CombineMeshes() {
//         ArrayList materials = new ArrayList();
//         ArrayList combineInstanceArrays = new ArrayList();
//         MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();

//         foreach (MeshFilter meshFilter in meshFilters) {
//             MeshRenderer meshRenderer = meshFilter.GetComponent<MeshRenderer>();

//             if (!meshRenderer ||
//                 !meshFilter.sharedMesh ||
//                 meshRenderer.sharedMaterials.Length != meshFilter.sharedMesh.subMeshCount) {
//                 continue;
//             }

//             for (int s = 0; s < meshFilter.sharedMesh.subMeshCount; s++) {
//                 int materialArrayIndex = Contains(materials, meshRenderer.sharedMaterials[s].name);
//                 if (materialArrayIndex == -1) {
//                     materials.Add(meshRenderer.sharedMaterials[s]);
//                     materialArrayIndex = materials.Count - 1;
//                 }
//                 combineInstanceArrays.Add(new ArrayList());

//                 CombineInstance combineInstance = new CombineInstance();
//                 combineInstance.transform = meshRenderer.transform.localToWorldMatrix;
//                 combineInstance.subMeshIndex = s;
//                 combineInstance.mesh = meshFilter.sharedMesh;
//                 (combineInstanceArrays[materialArrayIndex] as ArrayList).Add(combineInstance);
//             }
//         }

//         // Get / Create mesh filter & renderer
//         MeshFilter meshFilterCombine = gameObject.GetComponent<MeshFilter>();
//         if (meshFilterCombine == null) {
//             meshFilterCombine = gameObject.AddComponent<MeshFilter>();
//         }
//         MeshRenderer meshRendererCombine = gameObject.GetComponent<MeshRenderer>();
//         if (meshRendererCombine == null) {
//             meshRendererCombine = gameObject.AddComponent<MeshRenderer>();
//         }

//         // Combine by material index into per-material meshes
//         // also, Create CombineInstance array for next step
//         Mesh[] meshes = new Mesh[materials.Count];
//         CombineInstance[] combineInstances = new CombineInstance[materials.Count];

//         for (int m = 0; m < materials.Count; m++) {
//             CombineInstance[] combineInstanceArray = (combineInstanceArrays[m] as ArrayList).ToArray(typeof(CombineInstance)) as CombineInstance[];
//             meshes[m] = new Mesh();
//             meshes[m].CombineMeshes(combineInstanceArray, true, true);

//             combineInstances[m] = new CombineInstance();
//             combineInstances[m].mesh = meshes[m];
//             combineInstances[m].subMeshIndex = 0;
//         }

//         // Combine into one
//         meshFilterCombine.sharedMesh = new Mesh();
//         meshFilterCombine.sharedMesh.CombineMeshes(combineInstances, false, false);

//         // Destroy other meshes
//         foreach (Mesh oldMesh in meshes) {
//             oldMesh.Clear();
//             DestroyImmediate(oldMesh);
//         }

//         // Assign materials
//         Material[] materialsArray = materials.ToArray(typeof(Material)) as Material[];
//         meshRendererCombine.materials = materialsArray;

//         foreach (MeshFilter meshFilter in meshFilters) {
//             DestroyImmediate(meshFilter.gameObject);
//         }
//     }

//     private int Contains(ArrayList searchList, string searchName) {
//         for (int i = 0; i < searchList.Count; i++) {
//             if (((Material)searchList[i]).name == searchName) {
//                 return i;
//             }
//         }
//         return -1;
//     }
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour{
    
    public void CombineMeshes(){


        Quaternion oldRot = transform.rotation;
        Vector3 oldPos = transform.position;

        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;

        MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();

        Debug.Log (name + " is combining " + filters.Length + " meshes!");

        Mesh finalMesh = new Mesh ();

        CombineInstance[] combiners = new CombineInstance[filters.Length];

        for ( int a = 0; a < filters.Length; a++){
            if (filters [a].transform == transform)
                continue;

            combiners[a].subMeshIndex = 0;
            combiners[a].mesh = filters[a].sharedMesh;
            combiners[a].transform = filters[a].transform.localToWorldMatrix;
        }

        finalMesh.CombineMeshes(combiners);
        
        GetComponent<MeshFilter> ().sharedMesh = finalMesh;

        transform.rotation = oldRot;
        transform.position = oldPos;


        for (int a = 0; a < transform.childCount; a++)
            transform.GetChild (a).gameObject.SetActive(false);
    }



    // public void AdvancedMerge(){

    //     // All our children (and us)
    //     MeshFilter[] filters = GetComponentsInChildren<MeshFilter> (false);

    //     // All the meshes in our children (just a big list)
    //     List<Material> materials = new List<Material>();
    //     MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer> (false); // <-- you can optimize this
    //     foreach (MeshRenderer renderer in renderers){
    //         if (renderer.transform == transform)
    //            continue;
    //         Material[] localMats = renderer.sharedMaterials;
    //         foreach (Material localMat in localMats)
    //             if (!materials.Contains (localMat))
    //                 materials.Add (localMat);
    //     }

    //     // Each material will have a mesh for it.
    //     List<Mesh> submeshes = new List<Mesh>();
    //     foreach (Material material in materials){
    //         // Make a combiner for each (sub)mesh that is mapped to the right material.
    //         List<CombineInstance> combiners = new List<CombineInstance>();
    //         foreach (MeshFilter filter in filters){
    //             if (filter.transform == transform) continue;
    //             // The filter doesn't know what materials are involved, get the renderer.
    //             MeshRenderer renderer = filter.GetComponent<MeshRenderer>();    // <-- (Easy optimization is possible here, give it a try!)
    //             if (renderer == null){
    //                 Debug.LogError (filter.name + " has no MeshRenderer");
    //                 continue;
    //             }
        
    //             // Let's see if their materials are the one we want right now.
    //             Material[] localMaterials = renderer.sharedMaterials;
    //             for (int materialIndex = 0; materialIndex < localMaterials.Length; materialIndex++){
    //                 if (localMaterials [materialIndex] != material)
    //                    continue;
    //                 // This submesh is the material we're looking for right now.
    //                 CombineInstance ci = new CombineInstance();
    //                 ci.mesh = filter.sharedMesh;
    //                 ci.subMeshIndex = materialIndex;
    //                 ci.transform = Matrix4x4.identity;
    //                 combiners.Add (ci);
    //             }
    //         }
    //         // Flatten into a single mesh.
    //         Mesh mesh = new Mesh ();
    //         mesh.CombineMeshes (combiners.ToArray(), true);
    //         submeshes.Add (mesh);
    //     }

    //     // The final mesh: combine all the material-specific meshes as independent submeshes.
    //     List<CombineInstance> finalCombiners = new List<CombineInstance> ();
    //     foreach (Mesh mesh in submeshes) {
    //         CombineInstance ci = new CombineInstance ();
    //         ci.mesh = mesh;
    //         ci.subMeshIndex = 0;
    //         ci.transform = Matrix4x4.identity;
    //         finalCombiners.Add (ci);
    //     }
    //     Mesh finalMesh = new Mesh();
    //     finalMesh.CombineMeshes (finalCombiners.ToArray(), false);
    //     GetComponent<MeshFilter> ().sharedMesh = finalMesh;
    //     Debug.Log ("Final mesh has " + submeshes.Count + " materials.");
    // }
}
