using UnityEngine;
 
public static class LayerMaskExtensions{
 
    /// http://answers.unity.com/answers/1137700/view.html
    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}
