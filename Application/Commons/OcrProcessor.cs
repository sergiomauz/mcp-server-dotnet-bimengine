using System.Drawing;
using Tesseract;


namespace Application.Commons
{
    public class OcrProcessor
    {
        public string RunOcr(Bitmap bitmap)
        {
            // Path to the folder with Tesseract data (downloads the .traineddata) and language
            // Downloaded from https://github.com/tesseract-ocr/tessdata
            string tessDataPath = @"C:\revit-mcp-server\tesseract";
            string lang = "eng";

            //
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
