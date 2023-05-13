

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Scrapper
{
    class Driver
    {
       
        static void Main(string[] args)
        {

            ChromeDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://reports.blu.net/dailydata4.asp");

            string user = "ryan@capitalfuels.net";
            string password = "Rvwowwee_123";

            Console.WriteLine("Press Enter Once Logged In");
            Console.ReadLine();

           // Dictionary<string, string> loc_card = GetCardNum(driver);
            Dictionary<string, string> cust_name = new Dictionary<string, string>();

           // loc_card = loc_card.Keys.OrderBy(k => k).ToDictionary(Keys => Keys, k => loc_card[k]);//sort dictionary

            driver.Navigate().GoToUrl("https://user.petroleader.com/");//navigate to petroleader

            /*LOG IN*/
            driver.FindElement(By.XPath("/html/body/div[1]/section/div[1]/form/div/div[2]/div[2]/input")).SendKeys(user);//enter username
            driver.FindElement(By.XPath("/html/body/div[1]/section/div[1]/form/div/div[2]/div[4]/input")).SendKeys(password);//enter password
            driver.FindElement(By.XPath("/html/body/div[1]/section/div[1]/form/div/div[3]/input")).Click();//login
            Thread.Sleep(1000);
            
            //GO TO PAGE
            driver.FindElement(By.XPath("/html/body/div[1]/section/div/div[1]/div/ul/li[3]/a")).Click();//navigate to transaction reports
            Thread.Sleep(10);
                        
            /*SET DATE RANGE TO 7 DAYS*/
            driver.FindElement(By.XPath("/html/body/div[1]/section/form[1]/div/div/div/div[2]/div/div/div[1]/div[2]/span[1]/span/span/span")).Click();//open date range drop down
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("/html/body/div[10]/div/div[2]/ul/li[4]")).Click();//select 7 days

            
            SelectStation("Brooks", driver);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));//Wait for table to populate
            wait.Until(driver => driver.FindElement(By.ClassName("k-grid-content")));

            driver.FindElement(By.XPath("/html/body/div[1]/section/div[2]/div[1]/div[3]/span[2]/span/span/span[2]/span")).Click();//open table size menu
            Thread.Sleep(500);
            driver.FindElement(By.XPath("/html/body/div[18]/div/div[2]/ul/li[5]")).Click();//click 500

            wait.Until(driver => driver.FindElement(By.ClassName("k-grid-content")));//wait for table to populate

            var el = driver.FindElement(By.XPath("//*[text()='379550XXXXX7687=2702']"));//search for anything on the page that contains the card number

            var par = el.FindElement(By.XPath("./.."));//get that elements parent 
            var inputs = par.FindElements(By.TagName("input"));//get the children inputs

            Console.WriteLine(inputs.Count);
            inputs[2].Click();//open detail page

            driver.FindElement(By.XPath("/html/body/div[20]/div[2]/div/div/ul/li[2]/a")).Click();

        }

        public static void SelectStation(string station, ChromeDriver driver)
        {
            var loc = driver.FindElement(By.XPath("/html/body/div[1]/section/form[1]/div/div/div/div[1]/div/div/div[1]/div[2]/div/div"));
            loc.Click();

            Thread.Sleep(1000);

            string stationXPath = "";
            switch (station)
            {
                case "Brooks":
                    stationXPath = "/html/body/div[4]/div/div[2]/ul/li[1]";
                    break;
                case "Hurricane":
                    stationXPath = "/html/body/div[4]/div/div[2]/ul/li[2]"; 
                    break;
                case "Idaho Falls":
                    stationXPath = "/html/body/div[4]/div/div[2]/ul/li[3]";
                    break;
                case "Lamar":
                    stationXPath = "/html/body/div[4]/div/div[2]/ul/li[4]";
                    break;
                case "Myton":
                    stationXPath = "/html/body/div[4]/div/div[2]/ul/li[5]";
                    break;
                case "Plainfield":
                    stationXPath = "/html/body/div[4]/div/div[2]/ul/li[6]";
                    break;
                case "Sullivan":
                    stationXPath = "/html/body/div[4]/div/div[2]/ul/li[7]";
                    break;
                case "West Memphis":
                    stationXPath = "/html/body/div[4]/div/div[2]/ul/li[8]";
                    break;

            }
            driver.FindElement(By.XPath(stationXPath)).Click();
            driver.FindElement(By.XPath("/html/body/div[1]/section/form[1]/div/div/div/div[2]/div/div/div[8]/div[2]/input")).Click();//click load
        }

        public static Dictionary<string, string> GetCardNum(ChromeDriver driver)
        {
            Dictionary<string, string> customer = new Dictionary<string, string>();
            bool cont = true;
            Console.WriteLine("here");
            do
            {
                try
                {
                    var table = driver.FindElement(By.Id("fBody"));

                    foreach (var tr in table.FindElements(By.TagName("tr")))
                    {
                        var tds = tr.FindElements(By.TagName("td"));
                        Console.WriteLine(tds[0].Text + ":\t" + tds[5].Text);
                        customer.TryAdd(tds[0].Text, tds[5].Text);
                    }

                    var next = driver.FindElement(By.XPath("/html/body/div[3]/table[2]/tbody/tr/td[3]/h2/a"));
                    next.Click();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Table Not Found: END OF LIST?");
                    cont = false;
                }

            } while (cont);


            return customer;
        }
    }
}