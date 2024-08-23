using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;


const int scrollsCount = 3; // 1 scroll ~ 8 titles;
var chromeOptions = new ChromeOptions();
List<string> TitleWords = new List<string>();
chromeOptions.AddArgument("--start-maximized");



ChromeDriver driver = new ChromeDriver(chromeOptions);
Actions actions = new Actions(driver);
Dictionary<string, int> WordsCounter = new Dictionary<string, int>();

driver.Url = @"https://www.unian.net/war";

WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));


System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> titles;

do
{
    titles = driver.FindElements(By.ClassName("list-thumbs__title"));
    actions.MoveToElement(titles.Last());
    actions.Perform();

} while (titles.Count < 100);



foreach (IWebElement title in titles)
{
    actions.MoveToElement(title);
    actions.Perform();

    foreach (string word in Regex.Replace(title.Text, "[^A-Za-zА-Я-а-я _-]", "").Split())
    {
        if (word.Length > 2 || (word.Length == 2 && word.ToUpper() == word) )
        if(!WordsCounter.TryAdd(word, 1))
        {
            WordsCounter[word]++;
        }

    }

}

driver.Quit();


foreach (var item in WordsCounter.OrderBy(i => i.Value))
{
    Console.WriteLine($"{item.Key}: {item.Value}");
}



///////////////////////////// Абсолютно ничего не работает из нижеперечисленного, использовал не только то что ниже, но все равно принять куки у меня никак не получилось



//using (var driver = new ChromeDriver(chromeOptions))
//{
//    try
//    {
//        driver.Url = @"https://www.unian.net/war";
//        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

//        // Ожидаем загрузки страницы
//        wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

//        // Проверяем наличие кнопки и пытаемся нажать её различными способами
//        bool buttonClicked = false;

//        // Способ 1: Прямой клик через Selenium
//        try
//        {
//            var acceptButton = wait.Until(d => d.FindElement(By.CssSelector("button[data-button-type='acceptAll']")));
//            acceptButton.Click();
//            buttonClicked = true;
//            Console.WriteLine("Кнопка нажата через Selenium Click()");
//        }
//        catch (WebDriverTimeoutException)
//        {
//            Console.WriteLine("Не удалось найти кнопку для прямого клика");
//        }

//        // Способ 2: Клик через JavaScript
//        if (!buttonClicked)
//        {
//            try
//            {
//                var js = (IJavaScriptExecutor)driver;
//                var buttonExists = (bool)js.ExecuteScript("return !!document.querySelector('button[data-button-type=\"acceptAll\"]')");
//                if (buttonExists)
//                {
//                    js.ExecuteScript("document.querySelector('button[data-button-type=\"acceptAll\"]').click()");
//                    buttonClicked = true;
//                    Console.WriteLine("Кнопка нажата через JavaScript");
//                }
//                else
//                {
//                    Console.WriteLine("Кнопка не найдена через JavaScript");
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка при попытке нажать кнопку через JavaScript: {ex.Message}");
//            }
//        }

//        // Способ 3: Установка куки напрямую
//        if (!buttonClicked)
//        {
//            try
//            {
//                ((IJavaScriptExecutor)driver).ExecuteScript("document.cookie = 'cookieConsent=accepted; path=/;'");
//                Console.WriteLine("Установлен cookie для согласия на использование куки");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка при попытке установить cookie: {ex.Message}");
//            }
//        }

//        // Продолжаем выполнение остальной части вашего кода здесь...

//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Произошла неожиданная ошибка: {ex.Message}");
//    }
//}