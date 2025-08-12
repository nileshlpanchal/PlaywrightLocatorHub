using Deque.AxeCore.Commons;
using PlaywrightFramework.PageObjects.AccessibilityTesting;
using PlaywrightFramework.Utilities;
using Serilog;
using TechTalk.SpecFlow;

namespace PlaywrightFramework.StepDefinitions
{
    /// <summary>
    /// Step definitions for accessibility testing scenarios
    /// </summary>
    [Binding]
    public class AccessibilitySteps
    {
        private readonly TestPage _testPage;
        private AxeAccessibilityTester? _accessibilityTester;
        private AxeResult? _lastScanResults;
        private string? _lastReportPath;

        public AccessibilitySteps(TestPage testPage)
        {
            _testPage = testPage;
        }

        [Given(@"I initialize the accessibility tester")]
        public void GivenIInitializeTheAccessibilityTester()
        {
            try
            {
                Logger.Information("Initializing accessibility tester");
                _accessibilityTester = new AxeAccessibilityTester(_testPage.Page);
                Logger.Information("Accessibility tester initialized successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to initialize accessibility tester: {ex.Message}");
                throw;
            }
        }

        [When(@"I run a full accessibility scan")]
        public async Task WhenIRunAFullAccessibilityScan()
        {
            try
            {
                Logger.Information("Running full accessibility scan");
                
                if (_accessibilityTester == null)
                {
                    _accessibilityTester = new AxeAccessibilityTester(_testPage.Page);
                }

                _lastScanResults = await _accessibilityTester.RunFullAccessibilityScanAsync();
                Logger.Information($"Full accessibility scan completed with {_lastScanResults.Violations.Length} violations");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to run full accessibility scan: {ex.Message}");
                throw;
            }
        }

        [When(@"I run accessibility scan on the ""(.*)"" element with id ""(.*)""")]
        public async Task WhenIRunAccessibilityScanOnTheElementWithId(string elementType, string elementId)
        {
            try
            {
                Logger.Information($"Running accessibility scan on {elementType} element with id '{elementId}'");
                
                if (_accessibilityTester == null)
                {
                    _accessibilityTester = new AxeAccessibilityTester(_testPage.Page);
                }

                // Get the element locator based on type
                var element = elementType.ToLower() switch
                {
                    "textbox" => _testPage.Locators.GetTextboxById(elementId),
                    "button" => _testPage.Locators.GetButtonById(elementId),
                    "dropdown" => _testPage.Locators.GetDropdownById(elementId),
                    "checkbox" => _testPage.Locators.GetCheckboxById(elementId),
                    _ => _testPage.Locators.GetElementBySelector($"#{elementId}")
                };

                _lastScanResults = await _accessibilityTester.RunAccessibilityScanOnElementAsync(element);
                Logger.Information($"Element accessibility scan completed with {_lastScanResults.Violations.Length} violations");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to run accessibility scan on {elementType} element: {ex.Message}");
                throw;
            }
        }

        [When(@"I generate an accessibility report")]
        public async Task WhenIGenerateAnAccessibilityReport()
        {
            try
            {
                Logger.Information("Generating accessibility report");
                
                if (_lastScanResults == null)
                {
                    throw new InvalidOperationException("No accessibility scan results available. Run a scan first.");
                }

                if (_accessibilityTester == null)
                {
                    _accessibilityTester = new AxeAccessibilityTester(_testPage.Page);
                }

                _lastReportPath = await _accessibilityTester.GenerateAccessibilityReportAsync(_lastScanResults);
                Logger.Information($"Accessibility report generated: {_lastReportPath}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to generate accessibility report: {ex.Message}");
                throw;
            }
        }

        [Then(@"the accessibility scan should pass")]
        public void ThenTheAccessibilityScanShouldPass()
        {
            try
            {
                Logger.Information("Verifying accessibility scan passed");
                
                if (_lastScanResults == null)
                {
                    throw new InvalidOperationException("No accessibility scan results available");
                }

                var violationCount = _lastScanResults.Violations.Length;
                
                if (violationCount > 0)
                {
                    var violationSummary = string.Join(", ", _lastScanResults.Violations.Select(v => v.Id));
                    Logger.Error($"Accessibility scan failed with {violationCount} violations: {violationSummary}");
                    throw new Exception($"Accessibility scan failed with {violationCount} violations: {violationSummary}");
                }

                Logger.Information("Accessibility scan passed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Accessibility scan verification failed: {ex.Message}");
                throw;
            }
        }

        [Then(@"the accessibility scan should have no critical violations")]
        public void ThenTheAccessibilityScanShouldHaveNoCriticalViolations()
        {
            try
            {
                Logger.Information("Verifying no critical accessibility violations");
                
                if (_lastScanResults == null)
                {
                    throw new InvalidOperationException("No accessibility scan results available");
                }

                var criticalViolations = _lastScanResults.Violations
                    .Where(v => v.Impact?.ToLower() == "critical")
                    .ToArray();

                if (criticalViolations.Length > 0)
                {
                    var violationSummary = string.Join(", ", criticalViolations.Select(v => v.Id));
                    Logger.Error($"Found {criticalViolations.Length} critical accessibility violations: {violationSummary}");
                    throw new Exception($"Found {criticalViolations.Length} critical accessibility violations: {violationSummary}");
                }

                Logger.Information("No critical accessibility violations found");
            }
            catch (Exception ex)
            {
                Logger.Error($"Critical violations check failed: {ex.Message}");
                throw;
            }
        }

        [Then(@"the accessibility scan should have less than (.*) violations")]
        public void ThenTheAccessibilityScanShouldHaveLessThanViolations(int maxViolations)
        {
            try
            {
                Logger.Information($"Verifying accessibility scan has less than {maxViolations} violations");
                
                if (_lastScanResults == null)
                {
                    throw new InvalidOperationException("No accessibility scan results available");
                }

                var violationCount = _lastScanResults.Violations.Length;
                
                if (violationCount >= maxViolations)
                {
                    Logger.Error($"Accessibility scan found {violationCount} violations, expected less than {maxViolations}");
                    throw new Exception($"Accessibility scan found {violationCount} violations, expected less than {maxViolations}");
                }

                Logger.Information($"Accessibility scan passed with {violationCount} violations (less than {maxViolations})");
            }
            catch (Exception ex)
            {
                Logger.Error($"Accessibility violation count check failed: {ex.Message}");
                throw;
            }
        }

        [Then(@"the accessibility report should be generated")]
        public void ThenTheAccessibilityReportShouldBeGenerated()
        {
            try
            {
                Logger.Information("Verifying accessibility report was generated");
                
                if (string.IsNullOrEmpty(_lastReportPath))
                {
                    throw new InvalidOperationException("No accessibility report path available");
                }

                if (!File.Exists(_lastReportPath))
                {
                    throw new FileNotFoundException($"Accessibility report file not found: {_lastReportPath}");
                }

                var fileInfo = new FileInfo(_lastReportPath);
                if (fileInfo.Length == 0)
                {
                    throw new Exception("Accessibility report file is empty");
                }

                Logger.Information($"Accessibility report successfully generated: {_lastReportPath} ({fileInfo.Length} bytes)");
            }
            catch (Exception ex)
            {
                Logger.Error($"Accessibility report verification failed: {ex.Message}");
                throw;
            }
        }

        [Then(@"I should see accessibility results with (.*) violations")]
        public void ThenIShouldSeeAccessibilityResultsWithViolations(int expectedViolations)
        {
            try
            {
                Logger.Information($"Verifying accessibility results show exactly {expectedViolations} violations");
                
                if (_lastScanResults == null)
                {
                    throw new InvalidOperationException("No accessibility scan results available");
                }

                var actualViolations = _lastScanResults.Violations.Length;
                
                if (actualViolations != expectedViolations)
                {
                    Logger.Error($"Expected {expectedViolations} violations but found {actualViolations}");
                    throw new Exception($"Expected {expectedViolations} violations but found {actualViolations}");
                }

                Logger.Information($"Accessibility results verification successful: {actualViolations} violations");
            }
            catch (Exception ex)
            {
                Logger.Error($"Accessibility results verification failed: {ex.Message}");
                throw;
            }
        }
    }
}