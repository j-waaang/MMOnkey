namespace JYW.ThesisMMO.Common.GameWorld {
    class BoundingBox {

        public float Height { get; set; }
        public float Width { get; set; }

        public BoundingBox(float height, float width) {
            Height = height;
            Width = width;
        }
    }
}
