using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsciiLib {
    public class BitmapConv {
        public static Bitmap GetBitmap(string input, Font inputFont,
            Color bmpForeground, Color bmpBackground) {
            Image bmpText = new Bitmap(1, 1);
            try {
                // Measure the size of the text when applied to image.
                SizeF inputSize = TextRenderer.MeasureText(input, inputFont);

                // Create a new bitmap with the size of the text.
                bmpText = new Bitmap((int)inputSize.Width,
                    (int)inputSize.Height, PixelFormat.Format24bppRgb);

                // Instantiate graphics object, again, since our bitmap
                // was modified.
                Graphics g = Graphics.FromImage(bmpText);

                // Draw a background to the image.
                g.FillRectangle(new Pen(bmpBackground).Brush,
                    new Rectangle(0, 0,
                    Convert.ToInt32(inputSize.Width),
                    Convert.ToInt32(inputSize.Height)));
                // Draw the text to the image.
                TextRenderer.DrawText(g, input, inputFont,
                    new Point(0, 0), bmpForeground,
                    TextFormatFlags.Top |
                    TextFormatFlags.Left |
                    TextFormatFlags.NoPadding);
            } catch {
                // Draw a blank image with background.
                Graphics.FromImage(bmpText).FillRectangle(
                    new Pen(bmpBackground).Brush,
                    new Rectangle(0, 0, 1, 1));
            }

            return (Bitmap)bmpText;
        }

        public static Bitmap GetBitmapOld(string input, Font inputFont,
            Color bmpForeground, Color bmpBackground) {
            Image bmpText = new Bitmap(1, 1);
            try {
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
                    new Pen(bmpForeground).Brush, new Point(0, 0));
            } catch {
                // Draw a blank image with background.
                Graphics.FromImage(bmpText).FillRectangle(
                    new Pen(bmpBackground).Brush,
                    new Rectangle(0, 0, 1, 1));
            }

            return (Bitmap)bmpText;
        }
    }
}
