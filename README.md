# AsciiLib
An extraction of the code that Fonty2 used to convert text to ascii, only cleaner.

## Usage
You can use this very sexy one-liner to convert your text into "ascii":
```
string s = AsciiLib.AsciiConverter.CreateAscii("<text>", new Font("<font>", 20), "0", " ");
```

`charValue` is the character representing the "ascii" text.
`backValue` is the character representing the "background" -- usually leave it as space.

### In-depth usage
AsciiLib provides the ability to do your conversion in a two-fold operation:

```
public static Bitmap GetBitmap(string input, Font inputFont, Color bmpForeground, Color bmpBackground) {
```

This allows you to choose the colours of your generated bitmap (as well as grab the bitmap generated).

```
public static string GetAscii(Bitmap bmpInput, int rgbThreshold = 700, string strFont = "0", string strBackground = " ") {
```

This allows you to change the 'threshold' (though normally isn't needed, unless you choose to recolour the picture to create a gradient).
