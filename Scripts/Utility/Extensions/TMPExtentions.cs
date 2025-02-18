using TMPro;

namespace FxManager.UI.Extensions
{
    public static class TMPExtensions
    {
        public static TextMeshProUGUI AssignFontFromSource(this TextMeshProUGUI tmp, TextMeshProUGUI source, bool keepOrigColor = true)
        {
            if (tmp == null) return null;

            if (!keepOrigColor)
            {
                tmp.color = source.color;
                tmp.colorGradient = source.colorGradient;
                tmp.colorGradientPreset = source.colorGradientPreset;
            }
            tmp.font = source.font;
            tmp.fontMaterial = source.fontMaterial;
            tmp.fontStyle = source.fontStyle;
            tmp.fontWeight = source.fontWeight;
            tmp.outlineColor = source.outlineColor;
            tmp.outlineWidth = source.outlineWidth;
            tmp.faceColor = source.faceColor;

            return tmp;
        }
    }
}
