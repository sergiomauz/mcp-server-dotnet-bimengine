using System.Drawing;
using Tesseract;


namespace Application.Commons
{
    public class OcrProcessor
    {
        public string RunOcr(Bitmap bitmap)
        {
            string tessDataPath = @"C:\tessdata"; // Ruta a la carpeta con datos de Tesseract (descarga los .traineddata)
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
