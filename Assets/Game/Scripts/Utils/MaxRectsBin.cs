using System.Collections.Generic;
using UnityEngine;

public class MaxRectsBin
{
    public int Width;
    public int Height;

    public List<RectInt> FreeRectangles = new();
    public List<RectInt> UsedRectangles = new();

    public MaxRectsBin(int width, int height)
    {
        Width = width;
        Height = height;
        FreeRectangles.Add(new RectInt(0, 0, width, height));
    }

    public struct PlacedRect
    {
        public RectInt Rect;
        public bool Rotated;

        public PlacedRect(RectInt rect, bool rotated)
        {
            Rect = rect;
            Rotated = rotated;
        }
    }

    public PlacedRect Insert(int width, int height)
    {
        RectInt bestRect = new();
        bool bestRotated = false;
        int bestAreaFit = int.MaxValue;

        foreach (var freeRect in FreeRectangles)
        {
            // Try normal orientation
            if (width <= freeRect.width && height <= freeRect.height)
            {
                int areaFit = freeRect.width * freeRect.height - width * height;
                if (areaFit < bestAreaFit)
                {
                    bestRect = new RectInt(freeRect.x, freeRect.y, width, height);
                    bestAreaFit = areaFit;
                    bestRotated = false;
                }
            }

            // Try rotated orientation
            if (height <= freeRect.width && width <= freeRect.height)
            {
                int areaFit = freeRect.width * freeRect.height - width * height;
                if (areaFit < bestAreaFit)
                {
                    bestRect = new RectInt(freeRect.x, freeRect.y, height, width);
                    bestAreaFit = areaFit;
                    bestRotated = true;
                }
            }
        }

        if (bestAreaFit == int.MaxValue)
            return new PlacedRect(new RectInt(0, 0, 0, 0), false); // no fit

        PlaceRect(bestRect);
        return new PlacedRect(bestRect, bestRotated);
    }

    private void PlaceRect(RectInt rect)
    {
        for (int i = FreeRectangles.Count - 1; i >= 0; i--)
        {
            if (SplitFreeRect(FreeRectangles[i], rect))
                FreeRectangles.RemoveAt(i);
        }

        PruneFreeList();
        UsedRectangles.Add(rect);
    }

    private bool SplitFreeRect(RectInt freeRect, RectInt usedRect)
    {
        if (!freeRect.Overlaps(usedRect))
            return false;

        if (usedRect.x > freeRect.x && usedRect.x < freeRect.xMax)
        {
            FreeRectangles.Add(new RectInt(
                freeRect.x,
                freeRect.y,
                usedRect.x - freeRect.x,
                freeRect.height));
        }

        if (usedRect.xMax < freeRect.xMax)
        {
            FreeRectangles.Add(new RectInt(
                usedRect.xMax,
                freeRect.y,
                freeRect.xMax - usedRect.xMax,
                freeRect.height));
        }

        if (usedRect.y > freeRect.y && usedRect.y < freeRect.yMax)
        {
            FreeRectangles.Add(new RectInt(
                freeRect.x,
                freeRect.y,
                freeRect.width,
                usedRect.y - freeRect.y));
        }

        if (usedRect.yMax < freeRect.yMax)
        {
            FreeRectangles.Add(new RectInt(
                freeRect.x,
                usedRect.yMax,
                freeRect.width,
                freeRect.yMax - usedRect.yMax));
        }

        return true;
    }

    private void PruneFreeList()
    {
        for (int i = 0; i < FreeRectangles.Count; i++)
        {
            for (int j = i + 1; j < FreeRectangles.Count; j++)
            {
                if (IsContainedIn(FreeRectangles[i], FreeRectangles[j]))
                {
                    FreeRectangles.RemoveAt(i);
                    i--;
                    break;
                }

                if (IsContainedIn(FreeRectangles[j], FreeRectangles[i]))
                {
                    FreeRectangles.RemoveAt(j);
                    j--;
                }
            }
        }
    }

    private bool IsContainedIn(RectInt a, RectInt b)
    {
        return a.x >= b.x && a.y >= b.y &&
               a.xMax <= b.xMax && a.yMax <= b.yMax;
    }
}
