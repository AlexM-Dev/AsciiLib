using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiLib.Images {
    public class AsciiImageConverter {
        private static double ratio(Bitmap b){
            return b.Width / b.Height;
        }
        public static Bitmap ResizeOutput(Bitmap original, Bitmap input) {
            double r = ratio(original);

            int w = (int)(input.Height * r);

            return new Bitmap(input, new Size(w, input.Height));
        }
        public static string CreateAscii(Bitmap input, Font f, List<char> chars,
            int tolerance = 100, int channels = 4) {
            // Get all the colour values for every char in the list.
            // Use GetChars()
            var colours = colorValues(chars, tolerance, f);

            // Open new StringBuilder for appending.
            StringBuilder builder = new StringBuilder();

            // Open bitmap for retrieving pixels.
            LockBitmap lb = new LockBitmap(input);
            lb.LockBits();

            for (int y = 0; y < lb.Height; y++) {
                for (int x = 0; x < lb.Width; x++) {
                    Color c = lb.GetPixel(x, y);
                    // Get the closest average with the specified channels.
                    // div -> average. av -> reduce to b&w channels.
                    int div = (c.R + c.G + c.B) / 3;
                    int av = closestAv(div, channels);

                    // Get percentage 'density' of averaged pixel.
                    double perc = (av / 255d) * 100d;
                    // Get closest char to percentage.
                    var closest = colours.Aggregate((m, n) =>
                                Math.Abs(m.Value - perc) <
                                Math.Abs(n.Value - perc) ? m : n);
                    // Add to the string.
                    builder.Append(closest.Key);
                }
                builder.AppendLine();
            }
            lb.UnlockBits();

            // Return str.
            return builder.ToString();
        }
        public static List<char> GetChars() {
            return new HashSet<char>("0123456789abcdefghijklmnopqrst" +
                "uvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!\"#$%&'()*+,-./:;<=>?@[\\" +
                "]^_`{|}~".ToCharArray()).ToList();
        }
        private static int closestAv(int v, int c) {
            // Create list with values with channels in it.
            List<int> vals = new List<int>();
            for (int i = 0; i < 255; i += 255 / c) vals.Add(i);

            // White is an absolute necessity.
            if (!vals.Contains(255)) vals[vals.Count - 1] = 255;

            return vals.Aggregate((m, n) =>
                       Math.Abs(m - v) <
                       Math.Abs(n - v) ? m : n);
        }
        private static List<KeyValuePair<char, double>> colorValues(
            List<char> c, int d, Font f) {
            var values = new List<KeyValuePair<char, double>>();

            foreach (char ch in c)
                values.Add(new KeyValuePair<char, double>(ch, colorValue(ch, d, f)));

            for (int i = 0; i < values.Count; i++) {
                var item = values[i];
                item = new KeyValuePair<char, double>(item.Key,
                    Math.Round(((i + 1) / (double)values.Count) * 100, 4));
                values[i] = item;
            }

            values = values.OrderBy(x => x.Value).ToList();
            return values;
        }
        private static double colorValue(char c, int d, Font f) {
            int value = 0;
            Bitmap b = BitmapConv.GetBitmap(c.ToString(), f,
                Color.Black, Color.White);
            LockBitmap lb = new LockBitmap(b);
            lb.LockBits();

            // Process pixels.
            for (int x = 0; x < lb.Width; x++) {
                for (int y = 0; y < lb.Height; y++) {
                    Color clr = lb.GetPixel(x, y);

                    // Get total value of pixel.
                    int valXY = clr.R + clr.G + clr.B;
                    // If black, add. If white, don't.
                    value += valXY < d ? 1 : 0;
                }
            }

            lb.UnlockBits();

            // Return percentage.
            return (double)value / (b.Width * b.Height);
        }

    }
}
