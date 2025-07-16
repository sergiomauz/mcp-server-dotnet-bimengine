using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Tesseract;


namespace Application.Commons
{
    public class OcrProcessor
    {
        public Bitmap PreprocessImageGrayBW(Bitmap original)
        {
            // Convert to 24bpp to avoid stride and PixelFormat issues
            var sourceBitmap = new Bitmap(original.Width, original.Height, PixelFormat.Format24bppRgb);
            using (var g = Graphics.FromImage(sourceBitmap))
            {
                g.DrawImage(original, 0, 0, original.Width, original.Height);
            }

            var resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height, PixelFormat.Format24bppRgb);
            var rect = new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height);
            var sourceData = sourceBitmap.LockBits(rect, ImageLockMode.ReadOnly, sourceBitmap.PixelFormat);
            var resultData = resultBitmap.LockBits(rect, ImageLockMode.WriteOnly, resultBitmap.PixelFormat);
            var bytesPerPixel = Image.GetPixelFormatSize(sourceBitmap.PixelFormat) / 8;
            var height = sourceBitmap.Height;
            var stride = sourceData.Stride;
            var pixelBuffer = new byte[stride * height];
            var resultBuffer = new byte[stride * height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            for (int y = 0; y < height; y++)
            {
                int rowOffset = y * stride;

                for (int x = 0; x < sourceBitmap.Width; x++)
                {
                    // RGB
                    var idx = rowOffset + x * bytesPerPixel;
                    var blue = pixelBuffer[idx];
                    var green = pixelBuffer[idx + 1];
                    var red = pixelBuffer[idx + 2];

                    // Gray scale
                    var gray = (byte)(0.299 * red + 0.587 * green + 0.114 * blue);

                    // To binary
                    var binarized = (gray > 150) ? (byte)255 : (byte)0;
                    resultBuffer[idx] = binarized;
                    resultBuffer[idx + 1] = binarized;
                    resultBuffer[idx + 2] = binarized;
                }
            }
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);
            resultBitmap.UnlockBits(resultData);
            sourceBitmap.Dispose();

            return resultBitmap;
        }

        public string RunOcr(string tessDataPath, Bitmap bitmap)
        {
            // English
            string lang = "eng";
            using (var engine = new TesseractEngine(tessDataPath, lang, EngineMode.Default))
            {
                using (var ms = new MemoryStream())
                {
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    using (var pix = Pix.LoadFromMemory(ms.ToArray()))
                    {
                        using (var page = engine.Process(pix))
                        {
                            return page.GetText().Trim();
                        }
                    }
                }
            }
        }
    }
}
