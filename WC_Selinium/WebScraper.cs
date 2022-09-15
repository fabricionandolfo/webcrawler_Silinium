using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WC_Selinium
{
    internal class WebScraper
    {
        public static IWebDriver driver;
        private byte[] byteArray;

        public LogExtracao GetDados(string url, string alvo)
        {
            var caminho = @"C:\";
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("WCSelinium");

            LogExtracao log = new LogExtracao();
            List<Dado> dados = new List<Dado>();
            log.DataInicio = DateTime.Now.ToString("hh:mm:ss tt");

            driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);

            var collections = driver.FindElements(By.XPath(alvo));

            var comPaginas = driver.FindElements(By.XPath("/html/body/div[1]/div/div[2]/div/div/div/div[2]/nav/ul/li[position()>0]"));
            log.QuantPaginas = int.Parse(comPaginas.Last().Text);


            for (int i = 1; i <= log.QuantPaginas; i++)
            {
                if (i != 1)
                {
                    driver.Navigate().GoToUrl("https://proxyservers.pro/proxy/list/order/updated/order_dir/asc/page/" + i);
                    collections = driver.FindElements(By.XPath(alvo));

                }


                foreach (var item in collections)
                {

                    try
                    {
                        var D = new Dado
                        {
                            IpAdress = item.Text.Split(' ')[2],
                            Port = item.Text.Split(' ')[3],
                            Country = item.Text.Split(' ')[4],
                            Protocol = item.Text.Split(' ')[item.Text.Split(' ').Length - 2],
                        };


                        dados.Add(D);
                    }
                    catch (Exception)
                    {


                    }


                }
                print(i);
            }

            driver.Quit();
            log.QuantLinhas = dados.Count;
            log.DataFim = DateTime.Now.ToString("hh:mm:ss tt");
            Serializar("teste.json", dados);

            IMongoCollection<LogExtracao> logItem = database.GetCollection<LogExtracao>("log");

            logItem.InsertOne(log);

            return log;
        }

        public static void Serializar(string nomeArquivo, List<Dado> dados)
        {
            using (StreamWriter stream = new StreamWriter(Path.Combine(@"C:\", nomeArquivo)))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(stream, dados);
            }
        }

        private void print(int i)
        {
            try
            {
                System.Drawing.Bitmap printscreen = new System.Drawing.Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
                System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(printscreen as System.Drawing.Image);
                graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);

                byte[] byteArray = new byte[0];
                using (MemoryStream stream = new MemoryStream())
                {
                    printscreen.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    stream.Close();

                    byteArray = stream.ToArray();
                }

                File.WriteAllBytes(@"d:\\imagens\print" + i + ".png", byteArray);

            }
            catch (Exception erro)
            {

            }

        }
    }
}
