
using UnityEngine;

public class CharacterAdditional_Swim : MonoBehaviour
{

    public Transform origin; // the public transform you want to raycast from
    public float raycastDistance = 10f;
    public LayerMask waterLayer;
    public Vector3 raycastOffset;
    public bool onWater;

    public Transform waterBuoyancyTransform;
    public float floatingLerpSpeed;
    public float WaterSurfaceY;
    public float WaterBuoyancy;
    public float floatingOffset;

    private void Update()
    {
        // Cast a ray from origin with the given offset
        if (Physics.Raycast(origin.position + origin.TransformDirection(raycastOffset), origin.TransformDirection(Vector3.down), out RaycastHit hit, raycastDistance, waterLayer))
        {
            onWater = true;
            WaterSurfaceY = hit.point.y;
            WaterBuoyancy = waterBuoyancyTransform.position.y;
        }
        else
        {
            onWater = false;
            WaterSurfaceY = 0f;
            WaterBuoyancy = 0f;
        }

    }

    public bool GetSwimState()
    {
        return onWater;
    }

    public float GetWaterSurfaceInfo()
    {
        return WaterSurfaceY;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        // Draw the ray from origin with the given offset in the scene view
        Gizmos.DrawRay(origin.position + origin.TransformDirection(raycastOffset), origin.TransformDirection(Vector3.down) * raycastDistance);
    }


}
