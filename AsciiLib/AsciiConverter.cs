using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiLib {
    public class AsciiConverter {
        public static Bitmap GetBitmap(string input, Font inputFont,
            Color bmpForeground, Color bmpBackground) {
            Image bmpText = new Bitmap(1, 1);
            try {
                // Create a graphics object from the image.
                Graphics g = Graphics.FromImage(bmpText);

                // Measure the size of the text when applied to image.
                SizeF inputSize = g.MeasureString(input, inputFont);

                // Create a new bitmap with the size of the text.
                bmpText = new Bitmap((int)inputSize.Width,
                    (int)inputSize.Height);

                // Instantiate graphics object, again, since our bitmap
                // was modified.
                g = Graphics.FromImage(bmpText);

                // Draw a background to the image.
                g.FillRectangle(new Pen(bmpBackground).Brush,
                    new Rectangle(0, 0,
                    Convert.ToInt32(inputSize.Width),
                    Convert.ToInt32(inputSize.Height)));

                // Draw the text to the image.
                g.DrawString(input, inputFont,
                    new Pen(bmpForeground).Brush, new PointF(0, 0));
            } catch {
                // Draw a blank image with background.
                Graphics.FromImage(bmpText).FillRectangle(
                    new Pen(bmpBackground).Brush,
                    new Rectangle(0, 0, 1, 1));
            }

            return (Bitmap)bmpText;
        }

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
            Bitmap b = GetBitmap(input, font, Color.Black, Color.White);

            // Get ascii art of bitmap.
            return GetAscii(b, 700, charValue, backValue);
        }
    }
}
