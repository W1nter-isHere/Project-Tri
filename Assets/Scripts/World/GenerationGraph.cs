﻿using UnityEngine;
using XNode;

namespace World
{
    [CreateAssetMenu(fileName = "New Generation Graph", menuName = "Generation/New Terrain Generation Graph")]
    public class GenerationGraph : NodeGraph
    {
        [SerializeField] private bool enableDebugNodes;
        public bool EnableDebugNodes => enableDebugNodes;

    }
}