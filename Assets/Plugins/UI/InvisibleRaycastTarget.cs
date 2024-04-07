using UnityEngine;
using UnityEngine.UI;

namespace Dependencies.ChaserLib.UI
{
    /// A concrete subclass of the Unity UI `Graphic` class that just skips drawing.
    /// Useful for providing a raycast target without actually drawing anything.
    /// https://answers.unity.com/questions/1091618/ui-panel-without-image-component-as-raycast-target.html
    [RequireComponent(typeof(CanvasRenderer))]
    public class InvisibleRaycastTarget : Graphic
    {
        public override void SetMaterialDirty()
        {
        }

        public override void SetVerticesDirty()
        {
        }

        /// Probably not necessary since the chain of calls `Rebuild()`->`UpdateGeometry()`->`DoMeshGeneration()`->`OnPopulateMesh()` won't happen; so here really just as a fail-safe.
        protected override void OnPopulateMesh(VertexHelper vh) => vh.Clear();
    }
}