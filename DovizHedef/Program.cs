using HtmlAgilityPack;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Xml;




namespace DovizHedef
{
    public class Program
    {
        private static readonly HttpClient client = new HttpClient();
        public static double doviz;
        public static double kur;
        public static double masraf;
        public static double Kazanc;


        public static void Main(string[] args)
        {
            Console.SetWindowSize(100, 50);
            Console.Title = "Döviz Hedef";

            Console.WriteLine("Döviz Hedef Uygulamasına Hoşgeldiniz!");

            Console.Write("Döviz Miktarını Giriniz:");
            doviz = double.Parse(Console.ReadLine());

            Console.Write("Aldığınız Döviz Kurunu Giriniz:");
            kur = double.Parse(Console.ReadLine());

            Console.Write("Masraf Giriniz:");
            masraf = double.Parse(Console.ReadLine());

            Console.Write("Hedef Kazanc:");
            Kazanc = double.Parse(Console.ReadLine());

            // Wait for the user to hit <Enter>
            Timer t = new Timer(TimerCallback, null, 0, 15000);
            // Wait for the user to hit <Enter>
            Console.ReadLine();

        }

        private static void TimerCallback(Object o)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Green;


            Console.WriteLine("Güncelleme Zamanı: " + DateTime.Now);


            var a = Hesapla();



            GC.Collect();
        }

        public static double GetEnParaCurrency()
        {

            try
            {
                var url = "https://www.qnbfinansbank.enpara.com/hesaplar/doviz-ve-altin-kurlari";
                var web = new HtmlWeb();
                var doc = web.Load(url);

                var kur = doc.DocumentNode.SelectNodes("//div[@class='enpara-gold-exchange-rates__table-item']").First().InnerText.Replace(" USD ($)", "").Split(" TL ")[0].ToString().Trim();

                return double.Parse(kur);
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
        public static double Hesapla()
        {
            var EnParaKur = GetEnParaCurrency();


            double oncekiTutar = doviz * kur + masraf;

            double yeniTutar = EnParaKur * doviz + masraf;

            double hesaplanankazanc = yeniTutar - oncekiTutar;

            if (hesaplanankazanc >= Kazanc)
            {
           
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Limit Geçildi!! = " + hesaplanankazanc);


            }
            else
            {
                if (hesaplanankazanc < 0 )
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("↓ " +  hesaplanankazanc);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("↑ " + hesaplanankazanc);
                }
              
            }


            return hesaplanankazanc;
        }

      


    }



}
