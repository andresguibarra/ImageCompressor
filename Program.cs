using ImageProcessor;
using System;
using System.IO;

namespace CompresorImagenes
{
    class Program
    {
        static void Main(string[] args)
        {

            string filepath = Directory.GetCurrentDirectory();
            DirectoryInfo d = new DirectoryInfo(filepath);
            foreach (var file in d.GetFiles("*.jpg"))
            {
                Console.WriteLine("Comienza proceso de: " + file.Name);
                ComprimirImagen(file.FullName, file);
            }
            Console.WriteLine("Terminó todo el proceso");
            Console.ReadKey();
        }

        private static void ComprimirImagen(string fileName, FileInfo fi)
        {
            try
            {
                using (var inStream = new MemoryStream(File.ReadAllBytes(fileName)))
                {
                    using (var outStream = new MemoryStream())
                    {
                        using (var imageFactory = new ImageFactory())
                        {
                            int width = imageFactory.Load(inStream).Image.Width;
                            int height = imageFactory.Load(inStream).Image.Height;
                            int qualityFactor = 60;
                            if (width >= 2500)
                            {
                                width = width / 2;
                                height = height / 2;
                                imageFactory.Load(inStream).Resize(new System.Drawing.Size(width, height)).Quality(qualityFactor).Save(outStream);
                            }
                            else
                            {
                                imageFactory.Load(inStream).Quality(qualityFactor).Save(outStream);

                            }
                        }

                        FileStream fs = fi.OpenWrite();
                        fs.SetLength(outStream.Length);
                        outStream.CopyTo(fs);
                        fs.Close();
                        Console.WriteLine("Se procesó la imagen: " + fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error con imagen " + fileName + " : " + ex.Message);
            }
        }
    }
}
