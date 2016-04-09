using System;
using System.Collections.Generic;
using System.Configuration;
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
            Console.Title = "Carga masiva de archivos y/o nominas";
            try
            {
                var pathFolder = args[0].ToString();
                if (Directory.Exists(pathFolder.ToString()))
                    listarDirectoryFiles(pathFolder);
                else
                    Console.WriteLine("No existe el direcorio especificado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR => {0} - {1}", ex.Message.ToString());
            }
            Console.WriteLine("*********** Fin de proceso ****************");
            Environment.Exit(0);
        }

        private static void listarDirectoryFiles(string pathFolder)
        {
            var fileReader = new Logica.FileReader();
            Console.WriteLine("************** Iniciando Carga ****************");
            var listDirectoryFiles = new DirectoryInfo(pathFolder);
            Console.WriteLine("*************  Cantidad de archivos encontrados {0} son: {1} *******",pathFolder, listDirectoryFiles.EnumerateFiles().Count());
            fileReader.lineMessageLog.AppendLine(string.Format("******************** Inicio de carga {0} **************", DateTime.Now.ToString()));
            foreach (var file in listDirectoryFiles.GetFiles(string.Format("*.{0}", "CAM")))
            {
                var response = fileReader.loadFileAndSave(file.FullName); 
                Console.WriteLine(file.FullName);
            }
            fileReader.lineMessageLog.AppendLine("*************** fin de carga *****************");
            writeLog(fileReader.lineMessageLog);
        }
        private static void writeLog(StringBuilder lines) {
            try
            {
                var name = string.Format("info _{0}", DateTime.Now.ToString("yyyyMMdd"));
                var rootDirectory = ConfigurationManager.AppSettings["PathLog"].ToString();
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
