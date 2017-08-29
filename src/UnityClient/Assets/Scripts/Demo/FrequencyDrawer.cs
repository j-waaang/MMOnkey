using UnityEngine;
using System.Linq;
using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;
using System.Collections.Generic;

public class FrequencyDrawer : MonoBehaviour {

    private Texture2D m_MainTexture;
    private Color[,] m_DrawBoard;
    private int m_EntrySizeToRadiusFactor;

    private void Awake() {
        m_MainTexture = GetComponent<Renderer>().material.mainTexture as Texture2D;
        Debug.Assert(m_MainTexture.height == m_MainTexture.width);
        m_DrawBoard = new Color[m_MainTexture.height, m_MainTexture.height];
        ClearDrawboard();
        ClearTexture();
        m_MainTexture.Apply();
        EventOperations.FrequencyTableEvent += OnFreqTableUpdate;
        m_EntrySizeToRadiusFactor = (int) (m_MainTexture.height / 20);
    }

    private void OnFreqTableUpdate(IEnumerable<FrequencyEntry> entries) {
        ClearTexture();
        foreach (var entry in entries) {
            Debug.LogFormat("Min {0}, Max {1}, freq {2}", entry.MinDistance, entry.MaxDistance, entry.MilliSeconds);

            DrawRing((int)entry.MinDistance * m_EntrySizeToRadiusFactor,
                (int)entry.MaxDistance * m_EntrySizeToRadiusFactor,
                entry.MilliSeconds);
        }
        m_MainTexture.Apply();
    }

    private void TestDrawings() {
        ClearDrawboard();
        ClearTexture();
        DrawRing(80, 100, 30);
        DrawCircleFilled(110, 500, DrawMode.TextureIfGreener);
        m_MainTexture.Apply();
    }

    private void ApplyDrawboardToTexture() {
        for (int x = 0; x < m_MainTexture.height; x++) {
            for (int y = 0; y < m_MainTexture.height; y++) {
                DrawTextureIfGreener(x, y, m_DrawBoard[x, y]);
            }
        }
    }
    
    /// <summary>  
    ///  Draws a circle.
    ///  Used for testing only.
    /// </summary>  
    private void DrawCircle(int r) {
        int center = m_MainTexture.height / 2;

        float i, angle, x1, y1;

        for (i = 0; i < 360; i += 0.1f) {
            angle = i;
            x1 = r * Mathf.Cos(angle * Mathf.PI / 180);
            y1 = r * Mathf.Sin(angle * Mathf.PI / 180);

            m_MainTexture.SetPixel(center + (int)x1, center + (int)y1, new Color(235f/255f,20f/255f,0));
        }
    }

    /// <summary>  
    ///  Draws a ring to the drawing board. Drawmode is always force to avoid issues with inner ring.
    ///  Note: Drawing board should be applied to texture and filled before.
    /// </summary>  
    private void DrawRing(int innerRadius, int outerRadius, int freq) {
        if (innerRadius == 0) {
            DrawCircleFilled(outerRadius, freq, DrawMode.TextureIfGreener);
            return;
        }

        ClearDrawboard();
        DrawCircleFilled(outerRadius, freq, DrawMode.DrawboardOverride);
        DrawCircleFilled(innerRadius, -1, DrawMode.DrawboardOverride);
        ApplyDrawboardToTexture();
    }

    /// <summary>  
    ///  Draws a filled circle using the bresenham method.
    /// </summary>  
    private void DrawCircleFilled(int radius, int freq, DrawMode dm) {
        if(radius <= 0) { return; }

        int center = m_MainTexture.height / 2;
        var col = FreqToColor(freq);

        int error = -radius;
        int x = radius;
        int y = 0;

        while (x >= y) {
            int lastY = y;

            error += y;
            ++y;
            error += y;

            plot4points(center, center, x, lastY, col, dm);

            if (error >= 0) {
                if (x != lastY)
                    plot4points(center, center, lastY, x, col, dm);

                error -= x;
                --x;
                error -= x;
            }
        }
    }

    void plot4points(int cx, int cy, int x, int y, Color col, DrawMode dm) {
        horizontalLine(cx - x, cy + y, cx + x, col, dm);
        if (x != 0 && y != 0)
            horizontalLine(cx - x, cy - y, cx + x, col, dm);
    }

    void horizontalLine(int x0, int y0, int x1, Color col, DrawMode dm) {
        //Debug.LogFormat("hline y={0}, x0={1}, x1={2}", y0, x0, x1);
        for (int x = x0; x <= x1; ++x) {
            DrawPixel(x, y0, col, dm);
        }
    }

    private void DrawPixel(int x, int y, Color col, DrawMode dm) {
        switch (dm) {
            case DrawMode.DrawboardOverride:
                m_DrawBoard[x, y] = col;
                break;
            case DrawMode.TextureIfGreener:
                DrawTextureIfGreener(x, y, col);
                break;
        }
    }

    private void DrawTextureIfGreener(int x, int y, Color col) {
        var pixel = m_MainTexture.GetPixel(x, y);
        if (pixel.g >= col.g && pixel.a != 0) {
            return;
        }
        m_MainTexture.SetPixel(x, y, col);
    }

    private static Color FreqToColor(int freq) {
        //Debug.LogFormat("Freq2Col: freq: {0}", freq);
        if(freq == -1) { return Color.clear; }
        Debug.Assert(freq >= 0 && freq <= 500, freq);
        var val = (float)freq;
        val /= 510f;

        //Debug.LogFormat("Freq2Col, col: {0},{1},{2}", freq, 255 - freq, 0);

        return new Color(val, 1 - val, 0);
    }

    private enum DrawMode {
        DrawboardOverride,
        TextureIfGreener
    }

    private void ClearDrawboard() {
        for (int x = 0; x < m_DrawBoard.GetLength(0); x++) {
            for (int y = 0; y < m_DrawBoard.GetLength(1); y++) {
                m_DrawBoard[x, y] = Color.clear;
            }
        }
    }

    private void ClearTexture() {
        for (int x = 0; x < m_MainTexture.height; x++) {
            for (int y = 0; y < m_MainTexture.width; y++) {
                m_MainTexture.SetPixel(x, y, Color.clear);
            }
        }
    }
}
