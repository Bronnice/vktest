using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace VKTests
{
    [TestFixture]
    public class VKTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private const string BaseUrl = "https://vk.com";

        [SetUp]
        public void Setup()
        {
            // Инициализация драйвера Chrome
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            
            // Настройка неявного ожидания
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            
            // Настройка явного ожидания
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        [Test]
        public void Test1_PageTitleCheck()
        {
            // 1. Проверка заголовка страницы
            Console.WriteLine("Тест 1: Проверка заголовка страницы");
            
            driver.Url = BaseUrl;
            
            // Ожидание загрузки страницы
            wait.Until(d => d.Title.Contains("ВКонтакте"));
            
            string expectedTitle = "ВКонтакте | Добро пожаловать";
            string actualTitle = driver.Title;
            
            Assert.AreEqual(expectedTitle, actualTitle, 
                $"Заголовок страницы не соответствует ожидаемому. Ожидалось: {expectedTitle}, Фактически: {actualTitle}");
            
            Console.WriteLine($"✓ Заголовок страницы корректен: {actualTitle}");
        }

        [Test]
        public void Test2_ElementsVisibility()
        {
            // 2. Проверка видимости основных элементов
            Console.WriteLine("\nТест 2: Проверка видимости объектов");
            
            driver.Url = BaseUrl;
            
            // Проверка логотипа VK
            IWebElement logo = driver.FindElement(By.ClassName("VkIdForm__header"));
            Assert.IsTrue(logo.Displayed, "Логотип VK не отображается");
            Console.WriteLine("✓ Логотип VK отображается");

            // Проверка поля для ввода телефона/почты
            IWebElement loginField = driver.FindElement(By.Name("login"));
            Assert.IsTrue(loginField.Displayed, "Поле для ввода логина не отображается");
            Assert.IsTrue(loginField.Enabled, "Поле для ввода логина не активно");
            Console.WriteLine("✓ Поле для ввода логина доступно");

            // Проверка поля для ввода пароля
            IWebElement passwordField = driver.FindElement(By.Name("password"));
            Assert.IsTrue(passwordField.Displayed, "Поле для ввода пароля не отображается");
            Assert.IsTrue(passwordField.Enabled, "Поле для ввода пароля не активно");
            Console.WriteLine("✓ Поле для ввода пароля доступно");

            // Проверка кнопки "Войти"
            IWebElement loginButton = driver.FindElement(By.XPath("//button[@type='submit' and contains(@class, 'FlatButton')]"));
            Assert.IsTrue(loginButton.Displayed, "Кнопка 'Войти' не отображается");
            Assert.IsTrue(loginButton.Enabled, "Кнопка 'Войти' не активна");
            Console.WriteLine("✓ Кнопка 'Войти' доступна");
        }

        [Test]
        public void Test3_NavigationLinks()
        {
            // 3. Переход по ссылкам
            Console.WriteLine("\nТест 3: Переход по ссылкам");
            
            driver.Url = BaseUrl;
            
            // Переход по ссылке "Зарегистрироваться"
            IWebElement registerLink = wait.Until(d => 
                d.FindElement(By.XPath("//a[contains(@href, 'registration') or contains(text(), 'Зарегистрироваться')]")));
            
            string registerUrl = registerLink.GetAttribute("href");
            Assert.IsNotNull(registerUrl, "Ссылка регистрации не содержит URL");
            Console.WriteLine($"✓ Найдена ссылка регистрации: {registerUrl}");

            // Переход по ссылке "Восстановить пароль"
            IWebElement restoreLink = wait.Until(d => 
                d.FindElement(By.XPath("//a[contains(@href, 'restore') or contains(text(), 'Восстановить пароль')]")));
            
            string restoreUrl = restoreLink.GetAttribute("href");
            Assert.IsNotNull(restoreUrl, "Ссылка восстановления пароля не содержит URL");
            Console.WriteLine($"✓ Найдена ссылка восстановления пароля: {restoreUrl}");

            // Клик по ссылке регистрации (проверка перехода)
            registerLink.Click();
            
            // Ожидание загрузки страницы регистрации
            wait.Until(d => d.Url.Contains("registration") || d.Title.Contains("Регистрация"));
            Console.WriteLine("✓ Успешный переход по ссылке регистрации");
            
            // Возврат на главную страницу
            driver.Navigate().Back();
            wait.Until(d => d.Url == BaseUrl);
        }

        [Test]
        public void Test4_TextFieldsInput()
        {
            // 4. Заполнение текстовых полей
            Console.WriteLine("\nТест 4: Заполнение текстового поля");
            
            driver.Url = BaseUrl;
            
            // Находим поле для ввода логина
            IWebElement loginField = wait.Until(d => d.FindElement(By.Name("login")));
            
            // Очищаем поле
            loginField.Clear();
            
            // Вводим тестовый email
            string testEmail = "test@example.com";
            loginField.SendKeys(testEmail);
            
            // Проверяем, что текст введен корректно
            string enteredText = loginField.GetAttribute("value");
            Assert.AreEqual(testEmail, enteredText, "Текст в поле ввода не соответствует введенному");
            Console.WriteLine($"✓ В поле логина введен текст: {enteredText}");

            // Тестируем поле пароля
            IWebElement passwordField = driver.FindElement(By.Name("password"));
            passwordField.Clear();
            
            string testPassword = "TestPassword123";
            passwordField.SendKeys(testPassword);
            
            string enteredPassword = passwordField.GetAttribute("value");
            Assert.AreEqual(testPassword, enteredPassword, "Текст в поле пароля не соответствует введенному");
            Console.WriteLine("✓ В поле пароля успешно введен текст");
            
            // Очищаем поля
            loginField.Clear();
            passwordField.Clear();
            
            Assert.AreEqual("", loginField.GetAttribute("value"), "Поле логина не очистилось");
            Assert.AreEqual("", passwordField.GetAttribute("value"), "Поле пароля не очистилось");
            Console.WriteLine("✓ Текстовые поля успешно очищены");
        }

        [Test]
        public void Test5_ButtonClick()
        {
            // 5. Эмуляция нажатия на кнопку
            Console.WriteLine("\nТест 5: Эмуляция нажатия на кнопку");
            
            driver.Url = BaseUrl;
            
            // Заполняем поля некорректными данными
            IWebElement loginField = wait.Until(d => d.FindElement(By.Name("login")));
            IWebElement passwordField = driver.FindElement(By.Name("password"));
            
            loginField.Clear();
            passwordField.Clear();
            
            loginField.SendKeys("invalid_login");
            passwordField.SendKeys("invalid_password");
            Console.WriteLine("✓ Введены тестовые данные");
            
            // Находим и нажимаем кнопку "Войти"
            IWebElement loginButton = driver.FindElement(By.XPath("//button[@type='submit' and contains(@class, 'FlatButton')]"));
            
            Assert.IsTrue(loginButton.Enabled, "Кнопка 'Войти' не активна перед нажатием");
            Console.WriteLine("✓ Кнопка 'Войти' активна");
            
            // Запоминаем URL перед нажатием
            string urlBeforeClick = driver.Url;
            
            // Нажимаем кнопку
            loginButton.Click();
            Console.WriteLine("✓ Кнопка 'Войти' нажата");
            
            // Ожидаем изменения страницы (появление сообщения об ошибке или редирект)
            try
            {
                wait.Until(d => 
                    d.Url != urlBeforeClick || 
                    d.FindElements(By.XPath("//*[contains(text(), 'Неверный логин или пароль') or contains(text(), 'error')]")).Count > 0);
                
                Console.WriteLine("✓ Нажатие кнопки обработано системой");
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("⚠ Изменение страницы после нажатия кнопки не обнаружено (возможно, требуется капча)");
            }
        }

        [Test]
        public void Test6_ComplexScenario()
        {
            // Комплексный тест: все шаги вместе
            Console.WriteLine("\nТест 6: Комплексный сценарий");
            
            driver.Url = BaseUrl;
            
            // 1. Проверка заголовка
            Assert.IsTrue(driver.Title.Contains("ВКонтакте"), "Некорректный заголовок страницы");
            
            // 2. Проверка видимости элементов
            Assert.IsTrue(driver.FindElement(By.Name("login")).Displayed, "Поле логина не отображается");
            Assert.IsTrue(driver.FindElement(By.Name("password")).Displayed, "Поле пароля не отображается");
            
            // 3. Заполнение полей
            IWebElement loginField = driver.FindElement(By.Name("login"));
            IWebElement passwordField = driver.FindElement(By.Name("password"));
            
            loginField.SendKeys("test_user");
            passwordField.SendKeys("test_password");
            
            // 4. Проверка введенных данных
            Assert.AreEqual("test_user", loginField.GetAttribute("value"), "Логин введен некорректно");
            Assert.AreEqual("test_password", passwordField.GetAttribute("value"), "Пароль введен некорректно");
            
            // 5. Нажатие кнопки
            IWebElement loginButton = driver.FindElement(By.XPath("//button[@type='submit']"));
            loginButton.Click();
            
            Console.WriteLine("✓ Комплексный сценарий выполнен успешно");
        }

        [TearDown]
        public void TearDown()
        {
            // Делаем скриншот после каждого теста
            try
            {
                string testName = TestContext.CurrentContext.Test.Name;
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string screenshotPath = $"screenshot_{testName}_{timestamp}.png";
                
                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                screenshot.SaveAsFile(screenshotPath);
                Console.WriteLine($"Скриншот сохранен: {screenshotPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось сохранить скриншот: {ex.Message}");
            }
            
            // Закрываем браузер
            if (driver != null)
            {
                driver.Quit();
                driver = null;
            }
        }
    }
}