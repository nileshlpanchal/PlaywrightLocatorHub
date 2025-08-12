using Deque.AxeCore.Commons;
using Microsoft.Playwright;
using PlaywrightFramework.PageObjects.AccessibilityTesting;
using PlaywrightFramework.PageObjects;
using PlaywrightFramework.Utilities;
using Serilog;

namespace PlaywrightFramework.Tests
{
    /// <summary>
    /// NUnit test class for accessibility testing using Axe-core
    /// </summary>
    [TestFixture]
    public class AccessibilityTests
    {
        private IPlaywright? _playwright;
        private IBrowser? _browser;
        private IPage? _page;
        private TestPage? _testPage;
        private AxeAccessibilityTester? _accessibilityTester;
        private string _baseUrl = string.Empty;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Logger.Initialize();
            Logger.Information("Starting Accessibility Tests");
            _baseUrl = ConfigReader.GetTestSettings().BaseUrl;
        }

        [SetUp]
        public async Task SetUp()
        {
            _playwright = await Playwright.CreateAsync();
            
            var browserOptions = new BrowserTypeLaunchOptions
            {
                Headless = true,
                ExecutablePath = "/nix/store/zi4f80l169xlmivz8vja8wlphq74qqk0-chromium-125.0.6422.141/bin/chromium"
            };
            
            _browser = await _playwright.Chromium.LaunchAsync(browserOptions);
            _page = await _browser.NewPageAsync();
            _testPage = new TestPage(_page);
            _accessibilityTester = new AxeAccessibilityTester(_page);
            
            var testPageUrl = $"{_baseUrl}/TestData/SampleTestPage.html";
            await _testPage.NavigateToAsync(testPageUrl);
            
            Logger.Information("Accessibility test setup completed");
        }

        [TearDown]
        public async Task TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                await _testPage!.TakeScreenshotAsync($"accessibility-failed-{TestContext.CurrentContext.Test.Name}-{DateTime.Now:yyyyMMdd-HHmmss}.png");
                Logger.Information("Screenshot taken for failed accessibility test");
            }
            
            if (_page != null)
            {
                await _page.CloseAsync();
                _page = null;
            }
            
            if (_browser != null)
            {
                await _browser.CloseAsync();
                _browser = null;
            }
            
            if (_playwright != null)
            {
                _playwright.Dispose();
                _playwright = null;
            }
            
            Logger.Information("Accessibility test teardown completed");
        }

        [Test]
        public async Task FullPage_AccessibilityScan_ShouldHaveNoCriticalViolations()
        {
            // Act
            var results = await _accessibilityTester!.RunFullAccessibilityScanAsync();

            // Assert
            var criticalViolations = results.Violations.Where(v => v.Impact?.ToLower() == "critical").ToArray();
            
            Assert.Multiple(() =>
            {
                Assert.That(criticalViolations.Length, Is.EqualTo(0), 
                    $"Found {criticalViolations.Length} critical accessibility violations: {string.Join(", ", criticalViolations.Select(v => v.Id))}");
                Assert.That(results.Violations.Length, Is.LessThan(10), 
                    "Page should have fewer than 10 total accessibility violations");
            });

            Logger.Information($"Full page accessibility scan completed with {results.Violations.Length} violations");
        }

        [Test]
        [TestCase("firstName", "textbox")]
        [TestCase("lastName", "textbox")]
        [TestCase("email", "textbox")]
        [TestCase("submitBtn", "button")]
        [TestCase("country", "dropdown")]
        public async Task IndividualElement_AccessibilityScan_ShouldPass(string elementId, string elementType)
        {
            // Arrange
            var element = elementType.ToLower() switch
            {
                "textbox" => _testPage!.Locators.GetTextboxById(elementId),
                "button" => _testPage!.Locators.GetButtonById(elementId),
                "dropdown" => _testPage!.Locators.GetDropdownById(elementId),
                "checkbox" => _testPage!.Locators.GetCheckboxById(elementId),
                _ => _testPage!.Locators.GetElementBySelector($"#{elementId}")
            };

            // Act
            var results = await _accessibilityTester!.RunAccessibilityScanOnElementAsync(element);

            // Assert
            var criticalViolations = results.Violations.Where(v => v.Impact?.ToLower() == "critical").ToArray();
            
            Assert.That(criticalViolations.Length, Is.EqualTo(0),
                $"Element {elementId} has {criticalViolations.Length} critical accessibility violations");

            Logger.Information($"Element {elementId} accessibility scan completed with {results.Violations.Length} violations");
        }

        [Test]
        public async Task AccessibilityReport_Generation_ShouldSucceed()
        {
            // Arrange
            var results = await _accessibilityTester!.RunFullAccessibilityScanAsync();

            // Act
            var reportPath = await _accessibilityTester.GenerateAccessibilityReportAsync(results, 
                $"test-report-{TestContext.CurrentContext.Test.Name}-{DateTime.Now:yyyyMMdd-HHmmss}.html");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(File.Exists(reportPath), Is.True, "Accessibility report file should exist");
                
                var fileInfo = new FileInfo(reportPath);
                Assert.That(fileInfo.Length, Is.GreaterThan(0), "Accessibility report should not be empty");
                
                var content = File.ReadAllText(reportPath);
                Assert.That(content, Does.Contain("Accessibility Test Report"), "Report should contain title");
                Assert.That(content, Does.Contain("Summary"), "Report should contain summary section");
            });

            Logger.Information($"Accessibility report generated successfully: {reportPath}");
        }

        [Test]
        public async Task FormElements_AccessibilityValidation_ShouldPassStandards()
        {
            // Arrange
            var formElements = new[]
            {
                ("firstName", "textbox"),
                ("lastName", "textbox"), 
                ("email", "textbox"),
                ("password", "textbox"),
                ("country", "dropdown"),
                ("sports", "checkbox"),
                ("music", "checkbox"),
                ("submitBtn", "button")
            };

            var allViolations = new List<AxeResultItem>();

            // Act
            foreach (var (elementId, elementType) in formElements)
            {
                var element = elementType switch
                {
                    "textbox" => _testPage!.Locators.GetTextboxById(elementId),
                    "dropdown" => _testPage!.Locators.GetDropdownById(elementId),
                    "checkbox" => _testPage!.Locators.GetCheckboxById(elementId),
                    "button" => _testPage!.Locators.GetButtonById(elementId),
                    _ => _testPage!.Locators.GetElementBySelector($"#{elementId}")
                };

                try
                {
                    var results = await _accessibilityTester!.RunAccessibilityScanOnElementAsync(element);
                    allViolations.AddRange(results.Violations);
                }
                catch (Exception ex)
                {
                    Logger.Warning($"Could not scan element {elementId}: {ex.Message}");
                }
            }

            // Assert
            var criticalViolations = allViolations.Where(v => v.Impact?.ToLower() == "critical").ToArray();
            var seriousViolations = allViolations.Where(v => v.Impact?.ToLower() == "serious").ToArray();

            Assert.Multiple(() =>
            {
                Assert.That(criticalViolations.Length, Is.EqualTo(0), 
                    $"Form elements have {criticalViolations.Length} critical accessibility violations");
                Assert.That(seriousViolations.Length, Is.LessThanOrEqualTo(2), 
                    $"Form elements should have 2 or fewer serious accessibility violations, found {seriousViolations.Length}");
            });

            Logger.Information($"Form elements accessibility validation completed with {allViolations.Count} total violations");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Logger.Information("Accessibility Tests completed");
            Log.CloseAndFlush();
        }
    }
}