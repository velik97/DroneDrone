using UnityEngine;

namespace Util
{
    public static class Extensions
    {
        public static bool IsInMask(this LayerMask mask, GameObject gameObject)
        {
            return (mask & (1 << gameObject.layer)) != 0;
        }
    }
}