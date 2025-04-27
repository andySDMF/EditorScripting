using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BrandLab360.Editor
{

    public class CreateNavMeshContainerEditor : CreateBaseEditor
    {
        //[MenuItem("BrandLab360/Core/Navigation/Nav Mesh Container")]
        public static void Create()
        {
            NavMeshContainer nmContainer = new GameObject().AddComponent<NavMeshContainer>();
            nmContainer.transform.localScale = Vector3.one;
            nmContainer.transform.position = Vector3.zero;
            nmContainer.name = "NavMeshContainer_";

            GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.transform.SetParent(nmContainer.transform);
            plane.transform.localPosition = Vector3.zero;

            CreateParentAndSelectObject("_NAVMESH", nmContainer.transform);
        }
    }
}
