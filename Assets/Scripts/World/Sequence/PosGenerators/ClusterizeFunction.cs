﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace World.Sequence.PosGenerators
{
    public class ClusterizeFunction : FunctionNode
    {
        public override Type ProductType => typeof(Vector3Int[]);

        private Vector3Int[] _originalPositions;
        private Vector2Int _minSize;
        private Vector2Int _maxSize;
        private AnimationCurve _distanceEffect;
        private int _noiseScale;
        private int _noiseOctaves;
        private float _noiseThreshold;
        private ComparisonOperator _comparison;

        public ClusterizeFunction(Vector3Int[] originalPositions, Vector2Int minSize, Vector2Int maxSize, AnimationCurve distanceEffect, int noiseScale = 15, int noiseOctaves = 1, float noiseThreshold = 0.7f, ComparisonOperator comparison = ComparisonOperator.GreaterThan)
        {
            _originalPositions = originalPositions;
            _minSize = minSize;
            _maxSize = maxSize;
            _distanceEffect = distanceEffect;
            _noiseScale = noiseScale;
            _noiseOctaves = noiseOctaves;
            _noiseThreshold = noiseThreshold;
            _comparison = comparison;
        }

        protected override object SupplyInternal()
        {
            var poses = new List<Vector3Int>();
            var rand = new System.Random(Seed);
            
            foreach (var center in _originalPositions)
            {
                var size = new Vector2Int(rand.Next(_minSize.x, _maxSize.x + 1), rand.Next(_minSize.y, _maxSize.y + 1));

                var centreOfNoise = new Vector3Int((int)(size.x * 0.5f), (int)(size.y * 0.5f));
                var noise = NoiseHelper.GenerateNoiseMap(size.x, size.y, _noiseScale, offset: new Vector2(center.x, center.y), octave: _noiseOctaves, seed: Seed);
                
                for (var i = 0; i < size.x; i++)
                {
                    for (var j = 0; j < size.y; j++)
                    {
                        var pos = new Vector3Int(i, j);
                        var noiseValue = _distanceEffect.Evaluate(MathHelper.MapTo0_1(centreOfNoise.x, (pos - centreOfNoise).magnitude)) * noise[i, j];
                        if (!_comparison.Compare(_noiseThreshold, noiseValue)) continue;
                        var finalPos = pos + center - centreOfNoise;
                        if (finalPos.x >= XOffset && finalPos.x < Width + XOffset && finalPos.y >= YOffset && finalPos.y < Height + YOffset)
                        {
                            poses.Add(finalPos);
                        }
                    }
                }
            }

            return poses.ToArray();
        }
    }
}