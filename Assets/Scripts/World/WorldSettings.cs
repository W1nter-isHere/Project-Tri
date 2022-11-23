﻿namespace World
{
    public class WorldSettings
    {
        public static readonly WorldSettings Default = new WorldSettings("Test World", 0, 256, 256);
        
        public readonly string WorldName;
        public readonly int Seed;
        public readonly int Width;
        public readonly int Height;

        public WorldSettings(string worldName, int seed, int width, int height)
        {
            Seed = seed;
            Width = width;
            Height = height;
            WorldName = worldName;
        }

        public override string ToString()
        {
            return $"{WorldName}, Seed: {Seed}, Dimensions: {Width}x{Height}";
        }
    }
}