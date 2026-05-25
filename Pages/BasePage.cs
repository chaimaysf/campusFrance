// =============================================================================
// Page de base du POM (Page Object Model) : helpers réutilisables par toutes les pages
// =============================================================================

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FormRegister.Pages;

public abstract class BasePage
{
    protected readonly IWebDriver Driver;
    protected readonly WebDriverWait Wait;
    protected readonly IJavaScriptExecutor Js;

    protected BasePage(IWebDriver driver, TimeSpan? timeout = null)
    {
        Driver = driver;
        Wait = new WebDriverWait(driver, timeout ?? TimeSpan.FromSeconds(15));
        Js = (IJavaScriptExecutor)driver;
    }

    // Centre l'élément dans la fenêtre (utile si le header fixe cache le champ)
    protected void ScrollIntoView(IWebElement element) =>
        Js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);

    // Clic via JavaScript quand le clic Selenium normal est bloqué (overlay, header)
    protected void JsClick(IWebElement element) =>
        Js.ExecuteScript("arguments[0].click();", element);

    // Listes Selectize (pays, domaine, niveau…) : le vrai <select> est caché,
    // on ouvre le widget visible puis on clique l'option via son data-value du JSON
    protected void SelectSelectizeByDataValue(string wrapperCss, string dataValue)
    {
        var selectizeInput = Driver.FindElement(By.CssSelector($"{wrapperCss} .selectize-input"));
        ScrollIntoView(selectizeInput);
        Thread.Sleep(300);
        JsClick(selectizeInput);
        Thread.Sleep(500);

        var option = Wait.Until(d =>
            d.FindElement(By.CssSelector($".selectize-dropdown-content [data-value='{dataValue}']")));
        JsClick(option);
    }
}
