using System.Collections;
using TreeEditor;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale, int seed, int octaves, float persistence, float lacunarity, Vector2 offset)
    {
        // rtype
        float[,] noiseMap = new float[mapWidth, mapHeight];

        //Random seed for maps
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-1000, 1000) + offset.x;
            float offsetY = prng.Next(-1000, 1000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        //Catch 1/0 for scale
            if (scale <= 0)
        {
            scale = 0.0001f;
        }

        //Set min and max Values for bounds of normalization transform
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        //Make scale zoom into center of map - Ep2 19:31
        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        //Calculate NoiseMap[x,y] and Find min/max Values
        for(int y =0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {

                    float sampleX = (x-halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y-halfHeight) / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 -1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;
            }
        }

        //Normalize between min and max
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}
