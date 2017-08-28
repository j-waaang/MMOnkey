using UnityEngine;

public class FrequencyDrawer : MonoBehaviour {

    [SerializeField]
    private GameObject m_TargetPlane;
    private Texture2D m_MainTexture;

    private void Awake() {
        m_MainTexture = m_TargetPlane.GetComponent<Renderer>().material.mainTexture as Texture2D;
        ClearTexture();
        DrawCircleFilled(256, 256, 100);
        m_MainTexture.Apply(false);
    }

    private void ClearTexture() {
        for (int x = 0; x < m_MainTexture.height; x++) {
            for (int y = 0; y < m_MainTexture.height; y++) {
                m_MainTexture.SetPixel(x, y, Color.clear);
            }
        }
    }

    /// <summary>  
    ///  Draws a circle using the bresenham method.
    /// </summary>  
    private void DrawCircle(int x, int y, int r) {
        float i, angle, x1, y1;

        for (i = 0; i < 360; i += 0.1f) {
            angle = i;
            x1 = r * Mathf.Cos(angle * Mathf.PI / 180);
            y1 = r * Mathf.Sin(angle * Mathf.PI / 180);

            m_MainTexture.SetPixel(x + (int)x1, y + (int)y1, Color.blue);
        }
    }

    /// <summary>  
    ///  Draws a filled circle using the bresenham method.
    /// </summary>  
    private void DrawCircleFilled(int cx, int cy, int radius) {
        int error = -radius;
        int x = radius;
        int y = 0;

        while (x >= y) {
            int lastY = y;

            error += y;
            ++y;
            error += y;

            plot4points(cx, cy, x, lastY);

            if (error >= 0) {
                if (x != lastY)
                    plot4points(cx, cy, lastY, x);

                error -= x;
                --x;
                error -= x;
            }
        }
    }

    void plot4points(int cx, int cy, int x, int y) {
        horizontalLine(cx - x, cy + y, cx + x);
        if (x != 0 && y != 0)
            horizontalLine(cx - x, cy - y, cx + x);
    }

    void horizontalLine(int x0, int y0, int x1) {
        for (int x = x0; x <= x1; ++x) {
            m_MainTexture.SetPixel(x, y0, Color.blue);
        }
    }
}
