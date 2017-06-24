using UnityEngine;

public static class InputExtension {

    private static Plane m_GroundPlane = new Plane(Vector3.up, Vector3.zero);

    /// <summary>
    // Returns the point where the current mouse ray hits the ground plane.
    /// </summary>
    public static Vector3? GetMouseHitGroundPoint() {
        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitDistance;
        if(m_GroundPlane.Raycast(mouseRay, out hitDistance)) {
            var groundPoint = mouseRay.GetPoint(hitDistance);
            groundPoint.y = 0;
            return groundPoint;
        }
        return null;
    }
}
