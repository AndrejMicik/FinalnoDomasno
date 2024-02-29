using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using static SeleniumExtras.WaitHelpers.ExpectedConditions;
using SeleniumExtras.WaitHelpers;

namespace FinalnoDomasno
{
    [TestFixture]
    public class Class1
    {
        IWebDriver driver;
        WebDriverWait wait;
        Actions actions;
        string cargodomhomeurl = "http://16.170.177.179:9095/";
        string userEmail = "andrejmicik333@gmail.com";
        string userPassword = "brainsterqa1.18.2";
        string transporterEmail = "grom@abc.com";
        string transporterPassword = "hitnomitreba333";
        string requestTitle = "10 paleti Studiski sungeri";
        string fromAddress = "Belgrade Nikola Tesla Airport, Belgrade, Serbia";
        string toAddress = "Oktomvriska Revolucija, Kumanovo, North Macedonia";
        int length = Convert.ToInt32(50);
        int width = Convert.ToInt32(50);
        int height = Convert.ToInt32(50);
        int weight = Convert.ToInt32(10);
        int parcels = Convert.ToInt32(10);
        string studioPallets = "10 paleti Studiski sungeri";
        string fromCity = "RS\r\nBeograd";
        string toCity = "MK\r\nKumanovo";
        string requestWeightandVolume = "Вкупна тежина:\r\n1.100,00 kg\r\nВкупен волумен:\r\n0,00 m3";
        string requestType = "Палети";
        string requestTripLenght = "410 km";
        string requestStatus = "Транспортот е:\r\nАКТИВЕН";
        string requestStatusTransport = "Транспортот е:\r\nАКТИВЕН\r\nРезервирани кредити\r\n0,01";
        By loginButton = By.Id("login");
        By logOutButton = By.Id("logout2");
        double price = Convert.ToDouble(0.1);
        string messageToTheClient = "Ние сме уникатна транспорт компанија која ви гарантира, сигурност безбедност и навремена испорака на вашите производи и добра од секаков вид.";

        [SetUp]
        public void SetUpMethod()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(15);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            driver.Navigate().GoToUrl(cargodomhomeurl);
            actions = new Actions(driver);
        }
        [TearDown]
        public void TearDownMethod()
        {
            driver.Quit();
            driver.Dispose();
        }

        [Test]
        public void FirstOfManyTests()
        {
            IWebElement logInBtn = driver.FindElement(loginButton);
            logInBtn.Click();
            driver.FindElement(By.Id("username")).SendKeys(userEmail);
            driver.FindElement(By.Id("password")).SendKeys(userPassword);
            driver.FindElement(By.Id("rememberMe")).Click();
            driver.FindElement(By.ClassName("btn-green")).Click();

            IWebElement element = driver.FindElement(By.ClassName("main__sidenav"));
            List<IWebElement> leftNavElements = element.FindElements(By.CssSelector("li[ui-sref-active='active']")).ToList();
            IWebElement myProfileELement = leftNavElements.FirstOrDefault(el => el.Text.Contains("ОБЈАВИ БАРАЊЕ"));
            myProfileELement.Click();

            driver.FindElement(By.Name("title")).SendKeys(requestTitle);

            IWebElement categoryDropDownBtn = driver.FindElement(By.Id("field_y"));
            //categoryDropDownBtn.Click();
            SelectElement category = new SelectElement(categoryDropDownBtn);
            category.SelectByText("Палети");

            List<IWebElement> addressFields = driver.FindElements(By.CssSelector("input[ng-value='vm.address.formattedAddress']")).ToList();
            addressFields.First().SendKeys(fromAddress);
            wait.Until(ElementIsVisible(By.ClassName("pac-item")));
            addressFields.First().SendKeys(Keys.ArrowDown);
            addressFields.First().SendKeys(Keys.Enter);
            addressFields.Last().SendKeys(toAddress);
            wait.Until(ElementIsVisible(By.ClassName("pac-item")));
            addressFields.Last().SendKeys(Keys.ArrowDown);
            addressFields.Last().SendKeys(Keys.Enter);
            
            driver.FindElement(By.CssSelector("input[ng-model='dimension.length']")).SendKeys(length.ToString());
            driver.FindElement(By.CssSelector("input[ng-model='dimension.width']")).SendKeys(width.ToString());
            driver.FindElement(By.CssSelector("input[ng-model='dimension.height']")).SendKeys(height.ToString());
            driver.FindElement(By.CssSelector("input[ng-model='dimension.weight']")).SendKeys(weight.ToString());
            driver.FindElement(By.CssSelector("input[ng-model='dimension.quantity']")).SendKeys(parcels.ToString());
            driver.FindElement(By.Id("cachePickup")).Click();

            driver.FindElement(By.CssSelector("input[datetime-picker='dd.MM.yyyy HH:mm']")).Click();
            driver.FindElement(By.ClassName("glyphicon-chevron-right")).Click();
            driver.FindElement(By.ClassName("glyphicon-chevron-right")).Click();
            IWebElement calendar = driver.FindElement(By.ClassName("uib-daypicker"));
            List<IWebElement> weeksInJune = calendar.FindElements(By.ClassName("uib-weeks")).ToList();
            IWebElement ninethOfJune = weeksInJune.FirstOrDefault(el => el.Text.Contains("09"));
            ninethOfJune.Click();

            driver.FindElement(By.ClassName("photos-title")).Click();

            driver.FindElement(By.ClassName("center-block")).Click();

            IWebElement uploadRequest = driver.FindElement(By.CssSelector("input[type='submit']"));
            actions.ScrollToElement(uploadRequest);
            uploadRequest.Click();

            wait.Until(UrlToBe("http://16.170.177.179:9095/client/my-requests/active"));

            string requestLabel = driver.FindElement(By.ClassName("table-body__row")).FindElement(By.ClassName("column1")).Text;
            Assert.AreEqual(studioPallets, requestLabel);
            string startDestination = driver.FindElement(By.ClassName("table-body__row")).FindElement(By.ClassName("column2")).Text;
            Assert.AreEqual(fromCity, startDestination);
            string finalDestination = driver.FindElement(By.ClassName("table-body__row")).FindElement(By.ClassName("column3")).Text;
            Assert.AreEqual(toCity, finalDestination);
            string requestDescription = driver.FindElement(By.ClassName("table-body__row")).FindElement(By.ClassName("column4")).Text;
            Assert.AreEqual(requestWeightandVolume, requestDescription);
            string requestTypeDescription = driver.FindElement(By.ClassName("table-body__row")).FindElement(By.ClassName("column5")).Text;
            Assert.AreEqual(requestType, requestTypeDescription);
            string tripLenght = driver.FindElement(By.ClassName("table-body__row")).FindElement(By.ClassName("column6")).Text;
            Assert.AreEqual(requestTripLenght, tripLenght);
            string status = driver.FindElement(By.ClassName("table-body__row")).FindElement(By.ClassName("column7")).Text;
            Assert.AreEqual(requestStatus, status);

            driver.FindElement(logOutButton).Click();

            IWebElement logInBtn2 = driver.FindElement(loginButton);
            logInBtn2.Click();
            driver.FindElement(By.Id("username")).SendKeys(transporterEmail);
            driver.FindElement(By.Id("password")).SendKeys(transporterPassword);
            driver.FindElement(By.Id("rememberMe")).Click();
            driver.FindElement(By.ClassName("btn-green")).Click();

            IWebElement categoryDropDownBtn2 = driver.FindElement(By.CssSelector("select[ng-model='vm.searchParams.categoryType']"));
            categoryDropDownBtn2.Click();
            SelectElement category2 = new SelectElement(categoryDropDownBtn2);
            category2.SelectByIndex(4);
            driver.FindElement(By.CssSelector("a[ng-click='vm.clickSearch()']")).Click();

            IWebElement requestForTrans = wait.Until(ElementExists(By.LinkText("10 paleti Studiski sungeri")));
            requestForTrans.Click();
            driver.FindElement(By.ClassName("details-panel__make-offer-btn")).Click();
            driver.FindElement(By.CssSelector("input[ng-model='paymentType.price']")).SendKeys(price.ToString());

            driver.FindElement(By.CssSelector("input[datetime-picker='dd.MM.yyyy HH:mm']")).Click();
            driver.FindElement(By.ClassName("glyphicon-chevron-right")).Click();
            IWebElement calendar2 = driver.FindElement(By.ClassName("uib-daypicker"));
            List<IWebElement> weeksInMay = calendar2.FindElements(By.ClassName("uib-weeks")).ToList();
            IWebElement fifthOfMay = weeksInMay.FirstOrDefault(el => el.Text.Contains("05"));
            fifthOfMay.Click();
            driver.FindElement(By.ClassName("main__content")).Click();

            driver.FindElement(By.CssSelector("input[ng-click='vm.openExpirationDatePicker()']")).Click();
            driver.FindElement(By.ClassName("glyphicon-chevron-right")).Click();
            IWebElement calendar3 = driver.FindElement(By.ClassName("uib-daypicker"));
            List<IWebElement> endOfMay = calendar3.FindElements(By.ClassName("uib-weeks")).ToList();
            IWebElement thirtyFirstOfMay = endOfMay.FirstOrDefault(el => el.Text.Contains("31"));
            thirtyFirstOfMay.Click();
            driver.FindElement(By.ClassName("main__content")).Click();

            driver.FindElement(By.CssSelector("textarea[ng-model='vm.offerComment']")).SendKeys(messageToTheClient);
            driver.FindElement(By.ClassName("make-offer__btn-create")).Click();

            IWebElement confirmCreditDeduction = driver.FindElement(By.ClassName("modal-footer__btn-save"));
            confirmCreditDeduction.Click();

            driver.Navigate().GoToUrl("http://16.170.177.179:9095/provider/my-offers/active");

            string requestLabelAccept = driver.FindElement(By.ClassName("table-body__row")).FindElement(By.ClassName("column1")).Text;
            Assert.AreEqual(studioPallets, requestLabelAccept);
            string startDestinationAccept = driver.FindElement(By.ClassName("table-body__row")).FindElement(By.ClassName("column2")).Text;
            Assert.AreEqual(fromCity, startDestinationAccept);
            string finalDestinationAccept = driver.FindElement(By.ClassName("table-body__row")).FindElement(By.ClassName("column3")).Text;
            Assert.AreEqual(toCity, finalDestinationAccept);
            string requestDescriptionAccept = driver.FindElement(By.ClassName("table-body__row")).FindElement(By.ClassName("column4")).Text;
            Assert.AreEqual(requestWeightandVolume, requestDescriptionAccept);
            string requestTypeDescriptionAccept = driver.FindElement(By.ClassName("table-body__row")).FindElement(By.ClassName("column5")).Text;
            Assert.AreEqual(requestType, requestTypeDescriptionAccept);
            string tripLenghtAccept = driver.FindElement(By.ClassName("table-body__row")).FindElement(By.ClassName("column6")).Text;
            Assert.AreEqual(requestTripLenght, tripLenghtAccept);
            string statusAccept = driver.FindElement(By.ClassName("table-body__row")).FindElement(By.ClassName("column7")).Text;
            Assert.AreEqual(requestStatusTransport, statusAccept);

            driver.FindElement(logOutButton).Click();

            driver.FindElement(loginButton).Click();
            driver.FindElement(By.Id("username")).SendKeys(userEmail);
            driver.FindElement(By.Id("password")).SendKeys(userPassword);
            driver.FindElement(By.Id("rememberMe")).Click();
            driver.FindElement(By.ClassName("btn-green")).Click();

            driver.FindElement(By.CssSelector("a[ui-sref='client-my-requests']")).Click();
            driver.FindElement(By.CssSelector("a[ui-sref='client-request-details({id: request.id})']")).Click();

            IWebElement confirm = driver.FindElement(By.ClassName("flex-table__expander-btn"));
            actions.ScrollToElement(confirm);
            confirm.Click();
            IWebElement circle = driver.FindElement(By.Id("offer0"));
            actions.ScrollToElement(circle);
            circle.Click();
            IWebElement acceptOfferButton = driver.FindElement(By.CssSelector("input[value='Прифати понуда'"));
            acceptOfferButton.Click();

            Assert.IsTrue(wait.Until(UrlToBe("http://16.170.177.179:9095/client/accepted-offers/list")));
            List<IWebElement> detailsTables = driver.FindElements(By.CssSelector("tr[ng-repeat='request in vm.requests | filter:cargo']")).ToList();
            IWebElement accepted = detailsTables.FirstOrDefault(el => el.Text.Contains("ПРИФАТЕН"));
            Assert.IsTrue(accepted.Text.Contains("ПРИФАТЕН"));

            driver.FindElement(logOutButton).Click();
            Assert.IsTrue(wait.Until(UrlToBe("http://16.170.177.179:9095/")));

        }
    }
}