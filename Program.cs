using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;


namespace HtmlParsing
{
    class Program
    {

        static SqlConnection baglanConnection = new SqlConnection("Data Source=DESKTOP-3009CM5;Initial Catalog=instaVeri;Integrated Security=True");


        static void Main(string[] args)
        {
            Takipci();

            //Takip();
        }

        static void Takip()
        {
            IWebDriver driver = new ChromeDriver();

            Console.WriteLine("Kullanıcı adını girin :");
            string kullanici = Console.ReadLine();

            driver.Navigate().GoToUrl("https://www.instagram.com/" + kullanici + "/following/");

            System.Threading.Thread.Sleep(3000);

            driver.FindElement(By.Name("username")).SendKeys("hande_gecer@hotmail.com");
            System.Threading.Thread.Sleep(2000);


            System.Threading.Thread.Sleep(1000);

            driver.FindElement(By.Name("password")).SendKeys("886179");
            System.Threading.Thread.Sleep(1000);
            driver.FindElement(By.Name("password")).Submit();

            System.Threading.Thread.Sleep(1000);

            System.Threading.Thread.Sleep(4000);

            var donenSayi = TakipciSayi(driver);

            string textt;


            double donenSayi1 = 0;


            int sayac = 0;
            var icon = driver.FindElements(By.XPath("//*[@id=\"react-root\"]/section/main/div/header/section/ul/li/a"));

            foreach (IWebElement element in icon)
            {

                var text = element.Text;
                if (element.Text != (donenSayi.ToString() + " takipçi"))
                {
                    textt = driver
                       .FindElements(By.XPath("//*[@id=\"react-root\"]/section/main/div/header/section/ul/li/a/span"))[sayac].Text;
                    donenSayi1 = Convert.ToDouble(textt);

                    IWebElement page =
                        driver.FindElements(By.XPath("//*[@id=\"react-root\"]/section/main/div/header/section/ul/li/a"))[sayac];
                    page.Click();

                }

                sayac++;
            }

            double donenSayi2 = donenSayi1;



            System.Threading.Thread.Sleep(2000);


            System.Threading.Thread.Sleep(2000);



            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            System.Threading.Thread.Sleep(1000);

            double count = 0;

            while (donenSayi2 != count)
            {
                js.ExecuteScript("var followers = document.querySelector(\".isgrP\"); " +
                                 "followers.scrollBy(0,600);");
                System.Threading.Thread.Sleep(1000);
                count = driver.FindElements(By.XPath("/html/body/div/div/div/ul/div/li")).Count;

                System.Threading.Thread.Sleep(1000);
            }

            System.Threading.Thread.Sleep(2000);

            System.Threading.Thread.Sleep(2000);


            IReadOnlyCollection<IWebElement> searchResult =
                driver.FindElements(By.XPath("//a[@class='FPmhX notranslate  _0imsa ']"));


            foreach (var search in searchResult)
            {
                var t = search.Text;
                Console.WriteLine(t);
                VeriYazTakipci(t, "Takip", kullanici);
            }

        }


        private static double TakipciSayi(IWebDriver driver)
        {
            string textt = driver
                .FindElement(By.XPath("//*[@id=\"react-root\"]/section/main/div/header/section/ul/li/a/span")).Text;



            double donenSayi = Convert.ToDouble(textt);
            return donenSayi;
        }



        private static void Takipci()
        {
            IWebDriver driver = new ChromeDriver();

            Console.WriteLine("Kullanıcı adını girin :");
            string kullanici = Console.ReadLine();

            driver.Navigate().GoToUrl("https://www.instagram.com/" + kullanici + "/followers/");

            System.Threading.Thread.Sleep(3000);

            driver.FindElement(By.Name("username")).SendKeys("hande_gecer@hotmail.com");
            System.Threading.Thread.Sleep(2000);


            System.Threading.Thread.Sleep(1000);

            driver.FindElement(By.Name("password")).SendKeys("886179");
            System.Threading.Thread.Sleep(1000);
            driver.FindElement(By.Name("password")).Submit();

            System.Threading.Thread.Sleep(1000);

            System.Threading.Thread.Sleep(4000);

            IWebElement page = driver.FindElement(By.XPath("//*[@id=\"react-root\"]/section/main/div/header/section/ul/li/a"));
            System.Threading.Thread.Sleep(1000);
            page.Click();
            System.Threading.Thread.Sleep(2000);


            System.Threading.Thread.Sleep(2000);



            var donenSayi = TakipciSayi(driver);

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;


            double count = 0;

            while (donenSayi != count)
            {
                js.ExecuteScript("var followers = document.querySelector(\".isgrP\"); " +
                                 "followers.scrollBy(0,600);");
                //count = driver.FindElements(By.XPath("//a[@class='FPmhX notranslate _0imsa ']")).Count;
                count = driver.FindElements(By.XPath("/html/body/div/div/div/ul/div/li")).Count;
                System.Threading.Thread.Sleep(1000);
            }


            System.Threading.Thread.Sleep(2000);

            System.Threading.Thread.Sleep(2000);


            //IReadOnlyCollection<IWebElement> searchResult =
            //    driver.FindElements(By.XPath("//a[@class='FPmhX notranslate _0imsa ']"));


            IReadOnlyCollection<IWebElement> searchResult =
                driver.FindElements(By.XPath("//a[@class='FPmhX notranslate  _0imsa ']"));


            foreach (var search in searchResult)
            {
                var t = search.Text;
                Console.WriteLine(t);
                VeriYazTakipciler(t, "Takipçi",kullanici);
            }


            //int count = driver.FindElements(By.XPath("//a[@class='FPmhX notranslate _0imsa ']")).Count;
            //for (int i = 0; i < count; i++)
            //{
            //    Console.WriteLine(driver.FindElements(By.XPath("//a[@class='FPmhX notranslate _0imsa ']"))[i].Text);
            //}
        }

        static void VeriYazTakipciler(string kullanıcı, string Tip, string Kim)
        {

            baglanConnection.Open();

            SqlCommand komut = new SqlCommand("INSERT INTO db_Takipciler (VeriAd,VeriTip,Zaman,Kimden) VALUES('" + kullanıcı + "', '" + Tip + "','" + DateTime.Now.Date.ToString() + "','" + Kim + "')", baglanConnection);
            komut.ExecuteNonQuery();
            baglanConnection.Close();


        }

        static void VeriYazTakipci(string kullanıcı, string Tip, string Kim)
        {

            baglanConnection.Open();

            SqlCommand komut = new SqlCommand("INSERT INTO db_Takipler (VeriAd,VeriTip,Zaman,Kimden) VALUES('" + kullanıcı + "', '" + Tip + "','" + DateTime.Now.Date.ToString() + "','" + Kim + "')", baglanConnection);
            komut.ExecuteNonQuery();
            baglanConnection.Close();


        }

    }





}
