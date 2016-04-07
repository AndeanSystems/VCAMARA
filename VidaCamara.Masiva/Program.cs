using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidaCamara.Masiva
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ingresa la ruta donde se encuentra los archivos  de liquidaciones ó nominas a cargar");
            Console.ReadKey();
            var pathFolder = Console.ReadLine();
            try
            {
                if (Directory.Exists(pathFolder.ToString()))
                    listarDirectoryFiles(pathFolder);
                else
                    Console.WriteLine("No existe el direcorio especificado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR => {0} - {1}", ex.Message.ToString());
            }
            Console.ReadKey();
        }

        private static void listarDirectoryFiles(string pathFolder)
        {
            var fileReader = new Logica.FileReader();
            Console.WriteLine("************** Iniciando Carga ****************");
            var listDirectoryFiles = new DirectoryInfo(pathFolder);
            Console.WriteLine("*************  Cantidad de archivos encontrados {0} son: {1} *******",pathFolder, listDirectoryFiles.EnumerateFiles().Count());
            foreach (var file in listDirectoryFiles.GetFiles(string.Format("*.{0}", "CAM")))
            {
                var response = fileReader.loadFileAndSave(file.FullName);
                writeLog(fileReader.lineMessageLog);
                Console.WriteLine(file.FullName);
            }
        }
        private static void writeLog(StringBuilder lines) {
            try
            {
                var name = string.Format("info _{0}", DateTime.Now.ToString("yyyyMMdd"));
                var rootDirectory = @"C:\CargaMasiva\Log\";
                if (!Directory.Exists(rootDirectory))
                    Directory.CreateDirectory(rootDirectory);
                using (var file = File.AppendText(string.Format("{0}{1}.txt", rootDirectory,name )))
                {
                    file.WriteLine(lines);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
