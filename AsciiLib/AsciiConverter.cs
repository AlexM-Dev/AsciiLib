using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace AsciiLib {
    public class AsciiConverter { 
        public static string GetAscii(Bitmap bmpInput, int rgbThreshold = 700,
            string strFont = "0", string strBackground = " ") {
            // Create a new StringBuilder.
            StringBuilder inputConverted = new StringBuilder();

            // Lock the bitmap for editing.
            LockBitmap lockBitmap = new LockBitmap(bmpInput);
            lockBitmap.LockBits();
            for (int y = 0; y < lockBitmap.Height; y++) {
                for (int x = 0; x < lockBitmap.Width; x++) {
                    // Get the pixel value (R+G+B) = Total value.
                    Color pixel = lockBitmap.GetPixel(x, y);
                    int pixelValue = pixel.R + pixel.G + pixel.B;

                    // If value falls within the threshold, then
                    // append the font value, if not, the background value.
                    inputConverted.Append(pixelValue <= rgbThreshold ?
                        strFont : strBackground);
                }

                // Append a line, we're going left -> right, and
                // this is when we return to the next line.
                inputConverted.Append(Environment.NewLine);
            }
            lockBitmap.UnlockBits();

            // Return the string.
            return inputConverted.ToString();
        }

        public static string CreateAscii(string input, Font font,
            string charValue, string backValue) {
            // Get bitmap.
            Bitmap b = BitmapConv.GetBitmap(input, font,
                Color.Black, Color.White);

            // Get ascii art of bitmap.
            return GetAscii(b, 700, charValue, backValue);
        }
    }
}
