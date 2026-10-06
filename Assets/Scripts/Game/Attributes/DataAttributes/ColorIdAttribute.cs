using UnityEngine;

namespace Game.Sorcerum
{
    [System.Serializable]
    public class ColorIdAttribute : IDAttribute
    {
        [SerializeField] private Color _color;
        public override bool IsMatching(int idToCheck)
        {
            return idToCheck == AlwaysAccept || ColorIntConverter.ColorToInt(_color) == idToCheck;
        }

        public int GetId() => ColorIntConverter.ColorToInt(_color);
    }

    public static class ColorIntConverter
    {
        // Color -> Benzersiz int (RGBA formatında)
        public static int ColorToInt(Color color)
        {
            Color32 c = (Color32)color;
            // R (8-bit) | G (8-bit) | B (8-bit) | A (8-bit)
            return (c.r << 24) | (c.g << 16) | (c.b << 8) | c.a;
        }

        // int -> Color (Geri dönüştürme)
        public static Color IntToColor(int value)
        {
            byte r = (byte)((value >> 24) & 0xFF);
            byte g = (byte)((value >> 16) & 0xFF);
            byte b = (byte)((value >> 8) & 0xFF);
            byte a = (byte)(value & 0xFF);

            return new Color32(r, g, b, a);
        }
    }
}