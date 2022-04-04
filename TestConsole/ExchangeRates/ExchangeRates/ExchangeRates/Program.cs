using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;

namespace ExchangeRates
{
    class Program
    {
        static void Main(string[] args)
        {
            //Chiamata Api
            string apiUrl = "https://open.er-api.com/v6/latest/USD";

            //Stringa Json dalla chiamata Api
            string jsonString = "";

            //Nomi dei valori da convertire
            //Per ora di default, in futuro da far inserire all'utente
            string valueNameFrom = "EUR";
            string valueNameTo = "USD";

            //Variabile usata per il controllo se l'utente vuole continuare o no le conversioni
            bool checkContinue = false;

            //Chiamata Api
            Uri address = new Uri(apiUrl);
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "text/json";

            //Salvataggio su "jsonString" del Json String
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                jsonString = reader.ReadToEnd();
            }

            //Conversione da Json String a Json
            var test = JObject.Parse(jsonString);

            //Estrazione dei valori dal Json
            float valueFrom = float.Parse(test["rates"][valueNameFrom].ToString());
            float valueTo = float.Parse(test["rates"][valueNameTo].ToString());

            do
            {
                Console.Clear();

                Console.WriteLine($"Valore {valueNameFrom}: {valueFrom}");
                Console.WriteLine($"Valore {valueNameTo}: {valueTo}");

                Console.Write($"\nScrivi il valore da convertire da {valueNameFrom} a {valueNameTo}: ");
                float numberToConvert = float.Parse(Console.ReadLine());

                //Calcolo Conversione
                float result = numberToConvert * valueTo / valueFrom;

                Console.WriteLine($"\nIl risultato della conversione da {valueNameFrom} a {valueNameTo} equivale a {result} {valueNameTo}");

                Console.Write("\nVuoi Continuare? (Y/N) ");
                string continueOrNo = Console.ReadLine();

                //Controllo risposta dell'utente
                if (continueOrNo.ToLower().Equals("y") || continueOrNo.ToLower().Equals("yes") || continueOrNo.ToLower().Equals("s") || continueOrNo.ToLower().Equals("si"))
                {
                    //In caso la risposta fosse yes (o varianti), continua
                    checkContinue = false;
                }
                else
                {
                    //In caso la risposta fosse diversa dal yes, smette
                    checkContinue = true;
                }
            } while (checkContinue == false);

            Console.WriteLine("Fine");
        }
    }
}
