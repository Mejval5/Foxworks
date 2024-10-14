using Unity.Collections;
using UnityEngine;

namespace Foxworks.Voxels
{
    public struct HitMeshInfo
    {
        public Vector3 HitPoint;
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
        /// <param name="vertexAmount"></param>
        /// <param name="verticesValues"></param>
        /// <param name="gridOrigin"></param>
        /// <param name="enforceEmptyBorder"></param>
        /// <param name="hitExitWalls"></param>
        /// <returns></returns>
        public static HitMeshInfo RayMarch(Ray ray,
                                           float stepSize,
                                           float threshold,
                                           Vector3Int vertexAmount,
                                           NativeArray<int> verticesValues,
                                           Vector3 gridOrigin,
                                           bool enforceEmptyBorder,
                                           bool hitExitWalls = false)
        {
            HitMeshInfo hitInfo = new();
            Vector3 gridSpacing = new(1f, 1f, 1f);
            float maxDistance = Mathf.Sqrt(Mathf.Pow(vertexAmount.x * gridSpacing.x, 2) + Mathf.Pow(vertexAmount.y * gridSpacing.y, 2) + Mathf.Pow(vertexAmount.z * gridSpacing.z, 2));

            // Define the voxel grid bounds
            Vector3 gridMin = gridOrigin;
            Vector3 gridMax = gridOrigin + new Vector3(vertexAmount.x * gridSpacing.x, vertexAmount.y * gridSpacing.y, vertexAmount.z * gridSpacing.z);

            // Check if the ray intersects the voxel grid bounds
            if (!RayIntersectsBox(ray,
                                  gridMin,
                                  gridMax,
                                  out float tMin,
                                  out float tMax))
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

                float currentValue = SampleScalarField(currentPosition,
                                                       vertexAmount,
                                                       verticesValues,
                                                       gridOrigin,
                                                       gridSpacing,
                                                       enforceEmptyBorder);

                // Check for crossing
                if (!(currentValue > threshold))
                {
                    continue;
                }

                // Crossing detected, refine intersection point
                Vector3 startPoint = currentPosition - ray.direction * stepSize;
                Vector3 intersectionPoint = RefineIntersection(startPoint,
                                                               currentPosition,
                                                               threshold,
                                                               vertexAmount,
                                                               verticesValues,
                                                               gridOrigin,
                                                               gridSpacing,
                                                               enforceEmptyBorder);

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

        private static float SampleScalarField(Vector3 position,
                                               Vector3Int vertexAmount,
                                               NativeArray<int> verticesValues,
                                               Vector3 gridOrigin,
                                               Vector3 gridSpacing,
                                               bool enforceEmptyBorder)
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
            if (x < 0 || x > vertexAmount.x - 1 || y < 0 || y > vertexAmount.y - 1 || z < 0 || z > vertexAmount.z - 1)
            {
                return 0f; // Out of bounds is considered empty
            }
            
            if (enforceEmptyBorder && (x == 0 || x == vertexAmount.x - 1 || y == 0 || y == vertexAmount.y - 1 || z == 0 || z == vertexAmount.z - 1))
            {
                return 0f; // If the empty border is enforced, the border is considered empty
            }

            Vector3Int pos = new(x, y, z);

            if (Application.isEditor && Input.GetKey(KeyCode.O))
            {
                Debug.Log($"Position: {pos}, VertexAmount: {vertexAmount}");
            }

            // Get the scalar field value
            int index = MarchingCubeUtils.ConvertPositionToIndex(pos, vertexAmount);
            return VoxelDataUtils.UnpackValue(verticesValues[index]);
        }

        private static Vector3 RefineIntersection(Vector3 start,
                                                  Vector3 end,
                                                  float threshold,
                                                  Vector3Int vertexAmount,
                                                  NativeArray<int> verticesValues,
                                                  Vector3 gridOrigin,
                                                  Vector3 gridSpacing,
                                                  bool enforceEmptyBorder)
        {
            const int maxIterations = 10;
            const float tolerance = 1e-4f;

            Vector3 a = start;
            Vector3 b = end;

            float valueA = SampleScalarField(a,
                                             vertexAmount,
                                             verticesValues,
                                             gridOrigin,
                                             gridSpacing,
                                             enforceEmptyBorder) -
                           threshold;

            for (int i = 0; i < maxIterations; i++)
            {
                Vector3 samplePositionMid = (a + b) * 0.5f;
                float valueMid = SampleScalarField(samplePositionMid,
                                                   vertexAmount,
                                                   verticesValues,
                                                   gridOrigin,
                                                   gridSpacing,
                                                   enforceEmptyBorder) - threshold;

                if (Mathf.Abs(valueMid) < tolerance)
                {
                    return samplePositionMid;
                }

                if (valueA * valueMid < 0)
                {
                    b = samplePositionMid;
                }
                else
                {
                    a = samplePositionMid;
                    valueA = valueMid;
                }
            }

            return (a + b) * 0.5f;
        }
    }
}