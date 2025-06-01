using System;
using UnityEngine;

public class Grid<T>
{
    public T[,] Matrix { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public float CellSize { get; private set; }
    public Vector3 OriginPosition {  get; private set; }

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<T>, int, int, T> createGridObject)
    {
        this.Width = width;
        this.Height = height;
        this.CellSize = cellSize;
        this.OriginPosition = originPosition;

        Matrix = new T[width, height];

        for (int x = 0; x < Matrix.GetLength(0); x++)
        {
            for (int y = 0; y < Matrix.GetLength(1); y++)
            {
                Matrix[x, y] = createGridObject(this, x, y);
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * CellSize + OriginPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - OriginPosition).x / CellSize);
        y = Mathf.FloorToInt((worldPosition - OriginPosition).y / CellSize);
    }

    public void SetValue(int x, int y, T value)
    {
        if (x >= 0 && y >= 0 && x < Width && y < Height)
        {
            Matrix[x, y] = value;
        }
    }

    public void SetValue(Vector3 worldPosition, T value)
    {
        GetXY(worldPosition, out int x, out int y);
        SetValue(x, y, value);
    }

    public T GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < Width && y < Height)
        {
            return Matrix[x, y];
        }
        return default(T);
    }

    public T GetValue(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int y);
        return GetValue(x, y);
    }
}
