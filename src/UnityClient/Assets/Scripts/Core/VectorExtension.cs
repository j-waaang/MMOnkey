using JYW.ThesisMMO.Common.Types;
using UnityEngine;

public static class VectorExtension {

	public static Vector Vector3ToVector(Vector3 vector) {
        return new Vector(vector.x, vector.y, vector.z);
    }

    public static Vector3 VectorToVector3(Vector vector) {
        return new Vector3(vector.X, vector.Y, vector.Z);
    }
}
