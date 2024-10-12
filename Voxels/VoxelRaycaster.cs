using Unity.Collections;
using UnityEngine;

namespace Foxworks.Voxels
{
    public struct HitMeshInfo
    {
        public Vector3 HitPoint;
        public int VertexIndex;
        public bool IsHit;
        public bool HitExitWall;
        public Ray Ray;
    }

    /// <summary>
    ///     This class is used to cast rays into voxel grids.
    ///     The ray marching algorithm is used to find the intersection point of a ray with a scalar field.
    /// </summary>
    public static class VoxelRaycaster
    {
        /// <summary>
        ///     Ray marches through the voxel grid to find the intersection point of the ray with the scalar field.
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="stepSize"></param>
        /// <param name="threshold"></param>
        /// <param name="vertexAmountX"></param>
        /// <param name="vertexAmountY"></param>
        /// <param name="vertexAmountZ"></param>
        /// <param name="verticesValues"></param>
        /// <param name="gridOrigin"></param>
        /// <param name="hitExitWalls"></param>
        /// <returns></returns>
        public static HitMeshInfo RayMarch(
            Ray ray, 
            float stepSize, 
            float threshold, 
            int vertexAmountX, 
            int vertexAmountY, 
            int vertexAmountZ, 
            NativeArray<int> verticesValues, 
            Vector3 gridOrigin,
            bool hitExitWalls = false)
        {
            HitMeshInfo hitInfo = new();
            Vector3 gridSpacing = new(1f, 1f, 1f);
            float maxDistance = Mathf.Sqrt(Mathf.Pow(vertexAmountX * gridSpacing.x, 2) + Mathf.Pow(vertexAmountY * gridSpacing.y, 2) + Mathf.Pow(vertexAmountZ * gridSpacing.z, 2));

            // Define the voxel grid bounds
            Vector3 gridMin = gridOrigin;
            Vector3 gridMax = gridOrigin + new Vector3(vertexAmountX * gridSpacing.x, vertexAmountY * gridSpacing.y, vertexAmountZ * gridSpacing.z);

            // Check if the ray intersects the voxel grid bounds
            if (!RayIntersectsBox(ray, gridMin, gridMax, out float tMin, out float tMax))
            {
                hitInfo.IsHit = false;
                return hitInfo; // No intersection with the grid
            }

            // Adjust maxDistance to not exceed the distance inside the grid
            maxDistance = Mathf.Min(maxDistance, tMax - tMin);

            // Calculate the entry point using tMin
            Vector3 currentPosition = ray.origin + ray.direction * tMin;
            float distanceTraveled = 0f;

            while (distanceTraveled <= maxDistance)
            {
                currentPosition += ray.direction * stepSize;
                distanceTraveled += stepSize;

                float currentValue = SampleScalarField(currentPosition, vertexAmountX, vertexAmountY, vertexAmountZ, verticesValues, gridOrigin, gridSpacing);

                // Check for crossing
                if (!(currentValue > threshold))
                {
                    continue;
                }

                // Crossing detected, refine intersection point
                Vector3 intersectionPoint = RefineIntersection(ray, currentPosition - ray.direction * stepSize, currentPosition, threshold, vertexAmountX, vertexAmountY, vertexAmountZ, verticesValues,
                    gridOrigin, gridSpacing);

                if (Application.isEditor && Input.GetKey(KeyCode.P))
                {
                    Debug.DrawLine(ray.origin, intersectionPoint, Color.green, 10f);
                }

                hitInfo.HitPoint = intersectionPoint;
                hitInfo.IsHit = true;
                return hitInfo;
            }

            // No intersection found
            if (hitExitWalls == false)
            {
                hitInfo.IsHit = false;
                return hitInfo;
            }

            // Return the exit point
            hitInfo.HitPoint = currentPosition;
            hitInfo.IsHit = true;
            hitInfo.HitExitWall = true;
            return hitInfo;

        }

        private static bool RayIntersectsBox(Ray ray, Vector3 min, Vector3 max, out float tMin, out float tMax)
        {
            tMin = 0.0f;
            tMax = Mathf.Infinity;

            for (int i = 0; i < 3; i++)
            {
                float origin = ray.origin[i];
                float direction = ray.direction[i];
                float invD = 1.0f / direction;

                float t0 = (min[i] - origin) * invD;
                float t1 = (max[i] - origin) * invD;

                if (invD < 0.0f)
                {
                    // Swap t0 and t1
                    (t0, t1) = (t1, t0);
                }

                tMin = Mathf.Max(tMin, t0);
                tMax = Mathf.Min(tMax, t1);

                if (tMax <= tMin)
                {
                    return false; // No intersection
                }
            }

            return true;
        }

        private static float SampleScalarField(Vector3 position, int vertexAmountX, int vertexAmountY, int vertexAmountZ, NativeArray<int> verticesValues, Vector3 gridOrigin, Vector3 gridSpacing,
            bool detectSmallFeatures = false)
        {
            // Convert world position to grid indices
            Vector3 localPos = position - gridOrigin;
            localPos.x /= gridSpacing.x;
            localPos.y /= gridSpacing.y;
            localPos.z /= gridSpacing.z;
            int x = Mathf.RoundToInt(localPos.x);
            int y = Mathf.RoundToInt(localPos.y);
            int z = Mathf.RoundToInt(localPos.z);

            // Check bounds
            if (x < 0 || x >= vertexAmountX - 1 || y < 0 || y >= vertexAmountY - 1 || z < 0 || z >= vertexAmountZ - 1)
            {
                return 0f; // Or some default value
            }
            
            Vector3Int pos = new (x, y, z);
            Vector3Int vertexAmount = new (vertexAmountX, vertexAmountY, vertexAmountZ);
            
            // Get the scalar field value
            int index = MarchingCubeUtils.ConvertPositionToIndex(pos, vertexAmount);
            return VoxelDataUtils.UnpackValue(verticesValues[index]);
        }

        private static Vector3 RefineIntersection(Ray ray, Vector3 start, Vector3 end, float threshold, int vertexAmountX, int vertexAmountY, int vertexAmountZ, NativeArray<int> verticesValues,
            Vector3 gridOrigin, Vector3 gridSpacing)
        {
            const int maxIterations = 10;
            const float tolerance = 1e-4f;

            Vector3 a = start;
            Vector3 b = end;

            float valueA = SampleScalarField(a, vertexAmountX, vertexAmountY, vertexAmountZ, verticesValues, gridOrigin, gridSpacing) - threshold;

            for (int i = 0; i < maxIterations; i++)
            {
                Vector3 mid = (a + b) * 0.5f;
                float valueMid = SampleScalarField(mid, vertexAmountX, vertexAmountY, vertexAmountZ, verticesValues, gridOrigin, gridSpacing) - threshold;

                if (Mathf.Abs(valueMid) < tolerance)
                {
                    return mid;
                }

                if (valueA * valueMid < 0)
                {
                    b = mid;
                }
                else
                {
                    a = mid;
                    valueA = valueMid;
                }
            }

            return (a + b) * 0.5f;
        }
    }
}