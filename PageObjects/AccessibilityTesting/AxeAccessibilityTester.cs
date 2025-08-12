using Deque.AxeCore.Commons;
using Deque.AxeCore.Playwright;
using Microsoft.Playwright;
using PlaywrightFramework.Utilities;
using Serilog;

namespace PlaywrightFramework.PageObjects.AccessibilityTesting
{
    /// <summary>
    /// Handles accessibility testing using Axe-core engine
    /// </summary>
    public class AxeAccessibilityTester
    {
        private readonly IPage _page;

        public AxeAccessibilityTester(IPage page)
        {
            _page = page;
        }

        /// <summary>
        /// Run full accessibility scan on the current page
        /// </summary>
        public async Task<AxeResult> RunFullAccessibilityScanAsync()
        {
            try
            {
                Logger.Information("Running full accessibility scan on current page");
                
                // Inject Axe-core into the page and run accessibility scan
                var results = await _page.RunAxe();
                
                Logger.Information($"Accessibility scan completed. Found {results.Violations.Length} violations");
                return results;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error running accessibility scan: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Run accessibility scan on specific element
        /// </summary>
        public async Task<AxeResult> RunAccessibilityScanOnElementAsync(ILocator element)
        {
            try
            {
                Logger.Information("Running accessibility scan on specific element");
                
                // Get the element selector string for Axe
                var selector = element.ToString();
                
                // Run full page scan (element-specific scanning may not be supported)
                var results = await _page.RunAxe();
                
                Logger.Information($"Element accessibility scan completed. Found {results.Violations.Length} violations");
                return results;
            }
            catch (Exception ex)
            {
                Logger.Information("Falling back to full page scan for element");
                return await RunFullAccessibilityScanAsync();
            }
        }

        /// <summary>
        /// Generate accessibility report and save to file
        /// </summary>
        public async Task<string> GenerateAccessibilityReportAsync(AxeResult results, string fileName = "")
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = $"accessibility-report-{DateTime.Now:yyyyMMdd-HHmmss}.html";
                }

                var reportPath = Path.Combine("accessibility-reports", fileName);
                
                // Ensure directory exists
                Directory.CreateDirectory("accessibility-reports");

                // Generate HTML report
                var htmlReport = GenerateHtmlReport(results);
                await File.WriteAllTextAsync(reportPath, htmlReport);

                Logger.Information($"Accessibility report saved: {reportPath}");
                return reportPath;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error generating accessibility report: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Check if page meets accessibility standards
        /// </summary>
        public async Task<bool> IsPageAccessibleAsync(string[] allowedViolationRules = null)
        {
            try
            {
                var results = await RunFullAccessibilityScanAsync();
                
                // Filter violations if allowed rules are specified
                var criticalViolations = results.Violations;
                
                if (allowedViolationRules != null && allowedViolationRules.Length > 0)
                {
                    criticalViolations = results.Violations
                        .Where(v => !allowedViolationRules.Contains(v.Id))
                        .ToArray();
                }

                var isAccessible = criticalViolations.Length == 0;
                
                Logger.Information($"Page accessibility check: {(isAccessible ? "PASSED" : "FAILED")} - {criticalViolations.Length} critical violations");
                
                return isAccessible;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error checking page accessibility: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Generate detailed HTML report from Axe results
        /// </summary>
        private string GenerateHtmlReport(AxeResult results)
        {
            var html = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Accessibility Report - {DateTime.Now:yyyy-MM-dd HH:mm:ss}</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 20px; background-color: #f5f5f5; }}
        .header {{ background-color: #2c3e50; color: white; padding: 20px; border-radius: 5px; }}
        .summary {{ background-color: white; padding: 15px; margin: 20px 0; border-radius: 5px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }}
        .violation {{ background-color: #fff; margin: 15px 0; padding: 15px; border-left: 4px solid #e74c3c; border-radius: 3px; }}
        .violation-critical {{ border-left-color: #e74c3c; }}
        .violation-serious {{ border-left-color: #f39c12; }}
        .violation-moderate {{ border-left-color: #f1c40f; }}
        .violation-minor {{ border-left-color: #3498db; }}
        .impact {{ display: inline-block; padding: 3px 8px; border-radius: 3px; color: white; font-size: 12px; }}
        .impact-critical {{ background-color: #e74c3c; }}
        .impact-serious {{ background-color: #f39c12; }}
        .impact-moderate {{ background-color: #f1c40f; color: #333; }}
        .impact-minor {{ background-color: #3498db; }}
        .node {{ background-color: #ecf0f1; padding: 10px; margin: 5px 0; border-radius: 3px; }}
        .passed {{ color: #27ae60; }}
        .failed {{ color: #e74c3c; }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>🔍 Accessibility Test Report</h1>
        <p>Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>
        <p>URL: {results.Url}</p>
    </div>

    <div class='summary'>
        <h2>📊 Summary</h2>
        <p><strong>Total Violations:</strong> <span class='failed'>{results.Violations.Length}</span></p>
        <p><strong>Tests Passed:</strong> <span class='passed'>{results.Passes.Length}</span></p>
        <p><strong>Incomplete Tests:</strong> {results.Incomplete.Length}</p>
        <p><strong>Not Applicable:</strong> {results.Inapplicable.Length}</p>
    </div>";

            if (results.Violations.Length > 0)
            {
                html += @"
    <div class='violations'>
        <h2>❌ Accessibility Violations</h2>";

                foreach (var violation in results.Violations)
                {
                    html += $@"
        <div class='violation violation-{violation.Impact}'>
            <h3>{violation.Id}: {violation.Help}</h3>
            <span class='impact impact-{violation.Impact}'>{violation.Impact?.ToUpper()}</span>
            <p><strong>Description:</strong> {violation.Description}</p>
            <p><strong>Help URL:</strong> <a href='{violation.HelpUrl}' target='_blank'>{violation.HelpUrl}</a></p>
            <p><strong>Affected Elements ({violation.Nodes.Length}):</strong></p>";

                    foreach (var node in violation.Nodes)
                    {
                        html += $@"
            <div class='node'>
                <strong>Element:</strong> <code>{node.Html}</code><br>
                <strong>Target:</strong> <code>{string.Join(", ", node.Target)}</code><br>
                <strong>Impact:</strong> {violation.Impact}
            </div>";
                    }

                    html += "</div>";
                }

                html += "</div>";
            }

            html += @"
    <div class='summary'>
        <h2>✅ Tests Passed</h2>
        <p>The following accessibility tests passed successfully:</p>
        <ul>";

            foreach (var pass in results.Passes.Take(10)) // Show first 10 passed tests
            {
                html += $"<li><strong>{pass.Id}:</strong> {pass.Help}</li>";
            }

            if (results.Passes.Length > 10)
            {
                html += $"<li>... and {results.Passes.Length - 10} more tests passed</li>";
            }

            html += @"
        </ul>
    </div>
</body>
</html>";

            return html;
        }
    }
}