using System;
using System.Collections;
using UnityEngine;

public static class ActionHelper {

    public static IEnumerator WaitAndDo(float seconds, Action action) {
        yield return new WaitForSeconds(seconds);
        action();
    }
}
