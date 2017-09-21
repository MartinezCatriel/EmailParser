using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emailparser
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Comienzo del proceso");
            var pathFrom = string.Empty;
            var pathTo = string.Empty;

            var arguments = args.ToList();
            if (arguments.IndexOf("-from") != -1)
            {
                try
                {
                    pathFrom = arguments[arguments.IndexOf("-from") + 1].Equals("to") ? string.Empty : arguments[arguments.IndexOf("-from") + 1];
                }
                catch (Exception)
                {
                    Console.WriteLine("Falta enviar el origen");
                }
            }
            else
            {
                Console.WriteLine("Falta la ruta origen del archivo");
            }

            if (arguments.IndexOf("-to") != -1)
            {
                try
                {
                    pathTo = string.IsNullOrWhiteSpace(arguments[arguments.IndexOf("-to") + 1]) ? string.Empty : arguments[arguments.IndexOf("-to") + 1];
                }
                catch (Exception)
                {
                    Console.WriteLine("Falta enviar el destino");
                }
            }
            else
            {
                Console.WriteLine("Falta la ruta origen del archivo");
            }

            if (!string.IsNullOrWhiteSpace(pathFrom) && !string.IsNullOrWhiteSpace(pathTo))
            {
                var emails = ReadTextFile(pathFrom);
                WriteToFile(emails, pathTo);
            }
            Console.WriteLine("Fin del proceso");
        }

        private static void WriteToFile(List<EmailElement> emails, string toPath)
        {
            
            if (!emails.Any())
            {
                Console.WriteLine("Por algun motivo no hay mails");
                return;
            }
            Console.WriteLine("Comienzo de la escritura del archivo");
            try
            {
                FileStream fileStream = null;
                fileStream = File.Open(toPath, File.Exists(toPath) ? FileMode.Append : FileMode.OpenOrCreate);
                using (StreamWriter fs = new StreamWriter(fileStream))
                {
                    fs.WriteLine("Email");


                    foreach (var element in emails)
                    {

                        fs.WriteLine(element.Email);

                    }
                };
                fileStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al escribir el archivo. Error:{Newtonsoft.Json.JsonConvert.SerializeObject(ex)}");
            }
            Console.WriteLine("Finalizacion de la escritura del archivo");

        }

        private static List<EmailElement> ReadTextFile(string path)
        {
            try
            {
                Console.WriteLine("Comienzo de la lectura del archivo");

                var rtn = new List<EmailElement>();
                using (var file = new StreamReader(File.Open(path, FileMode.Open)))
                {
                    rtn = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EmailElement>>(file.ReadToEnd());
                }
                Console.WriteLine($"{rtn.Count} mails leidos");
                Console.WriteLine("Finalizacion de la lectura del archivo");
                return rtn;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al leer el archivo. Error:{Newtonsoft.Json.JsonConvert.SerializeObject(ex)}");
            }
            return new List<EmailElement>();
        }

    }

    public class EmailElement
    {
        public string Email { get; set; }
    }
}
