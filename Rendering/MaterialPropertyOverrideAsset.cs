using System.Collections.Generic;
using UnityEngine;

namespace Foxworks.Rendering
{
    [CreateAssetMenu(fileName = "MaterialPropertyOverrideAsset", menuName = "Assets/MaterialPropertyOverrideAsset")]
    public class MaterialPropertyOverrideAsset : ScriptableObject
    {
        public Shader shader;
        // This is where the overrides are serialized
        public List<MaterialPropertyOverride.ShaderPropertyValue> propertyOverrides = new();
    }
}