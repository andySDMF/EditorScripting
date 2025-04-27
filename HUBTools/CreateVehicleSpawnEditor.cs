using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class CreateVehicleSpawnEditor : CreateBaseEditor
    {
       // [MenuItem("BrandLab360/Vehicles/Spawn Point")]
        public static void CreateVehcileSpawn()
        {
            VehicleSpawn vhSpawn = new GameObject().AddComponent<VehicleSpawn>();
            vhSpawn.transform.localScale = Vector3.one;
            vhSpawn.transform.position = Vector3.zero;
            vhSpawn.name = "VehicleSpawn_";

            CreateParentAndSelectObject("_VEHICLES", vhSpawn.transform);
        }
    }
}
