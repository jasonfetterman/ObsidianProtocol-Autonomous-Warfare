// Project: WorldCreatorBridge
// Filename: Importer.cs
// Copyright (c) 2025 BiteTheBytes GmbH. All rights reserved
// *********************************************************

#region Using

using System.IO;
using UnityEngine;

#endregion

#if UNITY_EDITOR

namespace BtB.WC.Bridge
{
    public static class Importer
    {
        #region Methods (Static / Public)

        public static float[,] RawUint16FromFile(string filePath, int width, int height, bool bigEndian, int stride = 0,
            int headerSize = 0, bool flipY = false, bool flipX = false)
        {
            stride = stride == 0 ? width * 2 : stride;
            float[,] terrainData = new float[height, width];
            using (FileStream stream = File.OpenRead(filePath))
            {
                if (!bigEndian)
                {
                    for (int y = 0; y < height; y++)
                    {
                        stream.Position = y * stride + headerSize;
                        int readY = flipY ? (height - y) - 1 : y;
                        for (int x = 0; x < width; x++)
                        {
                            byte lower = (byte)stream.ReadByte();
                            byte upper = (byte)stream.ReadByte();
                            float val = (float)(lower | (upper << 8)) / ushort.MaxValue;
                            int readX = flipX ? (width - x) - 1 : x;

                            terrainData[readY, readX] = val;
                        }
                    }
                }
                else
                {
                    for (int y = 0; y < height; y++)
                    {
                        stream.Position = y * stride + headerSize;
                        int readY = flipY ? (height - y) - 1 : y;
                        for (int x = 0; x < width; x++)
                        {
                            byte upper = (byte)stream.ReadByte();
                            byte lower = (byte)stream.ReadByte();
                            float val = (float)(lower | (upper << 8)) / ushort.MaxValue;
                            int readX = flipX ? (width - x) - 1 : x;

                            terrainData[readY, readX] = val;
                        }
                    }
                }
            }

            return terrainData;
        }


        /// <summary>
        /// Loads a tga from the given filepath
        /// </summary>
        /// <param name="path"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Vector4[] ReadRGBA(string path, out int width, out int height)
        {
            using (FileStream fileStream = File.OpenRead(path))
            {
                using (BinaryReader reader = new BinaryReader(fileStream))
                {
                    reader.BaseStream.Seek(12, SeekOrigin.Begin);
                    width = reader.ReadInt16();
                    height = reader.ReadInt16();
                    int bitDepth = reader.ReadByte();
                    reader.BaseStream.Seek(1, SeekOrigin.Current);

                    int size = width * height;
                    Vector4[] textureData = new Vector4[size];
                    const float invByte = 1.0f / 255.0f;
                    if (bitDepth == 32)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            textureData[i] = new Vector4(
                                reader.ReadByte() * invByte,
                                reader.ReadByte() * invByte,
                                reader.ReadByte() * invByte,
                                reader.ReadByte() * invByte);
                        }
                    }
                    else if (bitDepth == 24)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            textureData[i] = new Vector4(
                                reader.ReadByte() * invByte,
                                reader.ReadByte() * invByte,
                                reader.ReadByte() * invByte, 1);
                        }
                    }
                    else if (bitDepth == 8)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            float v = reader.ReadByte() * invByte;
                            textureData[i] = new Vector4(
                                v, 
                                v, 
                                v, 
                                1);
                        }
                    }
                    else
                        return null;

                    return textureData;
                }
            }
        }

        #endregion
    }
}

#endif