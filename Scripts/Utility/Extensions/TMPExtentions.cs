using TMPro;

namespace FxManager.UI.Extensions
{
    public static class TMPExtensions
    {
        public static TextMeshProUGUI AssignFontFromSource(this TextMeshProUGUI tmp, TextMeshProUGUI source, bool keepOrigColor = true)
        {
            tmp.font = source.font;
            tmp.material = source.material;

            return tmp;
        }
    }
}
