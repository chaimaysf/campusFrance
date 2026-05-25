// =============================================================================
// Page Object : formulaire d'inscription Campus France
// Contient les sélecteurs et actions Selenium (pas de Gherkin ici)
// =============================================================================

using FormRegister.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FormRegister.Pages;

public class InscriptionPage : BasePage
{
    // #user-form limite la recherche au formulaire (évite les doublons d'id sur la page)
    private const string EmailField = "#user-form #edit-name";
    private const string SubmitButton = "#user-form #edit-submit";

    public InscriptionPage(IWebDriver driver) : base(driver) { }

    // Ouvre l'URL + gère le bandeau cookies Tarteaucitron
    public void Open()
    {
        Driver.Navigate().GoToUrl(TestUrls.Inscription);
        Thread.Sleep(2000);
        AcceptCookiesIfPresent();
    }

    // Champs communs aux 3 profils (étudiant, chercheur, institutionnel)
    public void FillCommonFields(Client client)
    {
        // Email : JS car SendKeys seul provoque souvent ElementNotInteractable
        var emailField = Wait.Until(d => d.FindElement(By.CssSelector(EmailField)));
        Js.ExecuteScript(
            "arguments[0].scrollIntoView({block:'center'}); arguments[0].value=arguments[1]; arguments[0].dispatchEvent(new Event('input',{bubbles:true}));",
            emailField,
            client.Email);

        Driver.FindElement(By.Id("edit-pass-pass1")).SendKeys(client.Password);
        Driver.FindElement(By.Id("edit-pass-pass2")).SendKeys(client.ConfirmPassword);

        var civilityId = client.Civility == "Mr" ? "edit-field-civilite-mr" : "edit-field-civilite-mme";
        JsClick(Driver.FindElement(By.Id(civilityId)));

        Driver.FindElement(By.Id("edit-field-nom-0-value")).SendKeys(client.LastName);
        Driver.FindElement(By.Id("edit-field-prenom-0-value")).SendKeys(client.FirstName);

        // Pays : Selectize — countryOfResidenceValue dans le JSON (ex. "109")
        SelectSelectizeByDataValue("#edit-field-pays-concernes-wrapper", client.CountryOfResidenceValue);

        Driver.FindElement(By.Id("edit-field-nationalite-0-target-id")).SendKeys(client.Nationality);
        Driver.FindElement(By.Id("edit-field-code-postal-0-value")).SendKeys(client.PostCode);
        Driver.FindElement(By.Id("edit-field-ville-0-value")).SendKeys(client.City);
        Driver.FindElement(By.Id("edit-field-telephone-0-value")).SendKeys(client.Phone);
    }

    // Profil Étudiant : case à cocher + domaine + niveau d'études
    public void FillStudentProfile(Client client)
    {
        FillCommonFields(client);
        JsClick(Driver.FindElement(By.Id("edit-field-publics-cibles-2")));
        SelectSelectizeByDataValue("#edit-field-domaine-etudes-wrapper", client.studyFieldValue);
        SelectSelectizeByDataValue("#edit-field-niveaux-etude-wrapper", client.studyLevelValue);
        AcceptCommunications();
    }

    // Profil Chercheur : autre case profil (XPath) + domaine + niveau
    public void FillSearcherProfile(Client client)
    {
        FillCommonFields(client);

        var profilBtn = Driver.FindElement(By.XPath("//*[@id=\"edit-field-publics-cibles\"]/div[2]"));
        ScrollIntoView(profilBtn);
        Thread.Sleep(300);
        profilBtn.Click();

        SelectSelectizeByDataValue("#edit-field-domaine-etudes-wrapper", client.studyFieldValue);
        SelectSelectizeByDataValue("#edit-field-niveaux-etude-wrapper", client.studyLevelValue);
        AcceptCommunications();
    }

    // Profil Institutionnel : fonction + type organisme + nom organisme
    public void FillInstitutionnelProfile(Client client)
    {
        FillCommonFields(client);

        var profilBtn = Driver.FindElement(By.Id("edit-field-publics-cibles-4"));
        ScrollIntoView(profilBtn);
        Thread.Sleep(300);
        JsClick(profilBtn);

        Driver.FindElement(By.Id("edit-field-fonction-0-value")).SendKeys(client.Function);
        SelectSelectizeByDataValue("#edit-field-type-organisme-wrapper", client.OrganizationTypeValue);
        Driver.FindElement(By.Id("edit-field-nom-organisme-0-value")).SendKeys(client.OrganizationName);
        AcceptCommunications();
    }

    // Case obligatoire : accepter les communications Campus France
    public void AcceptCommunications() =>
        Driver.FindElement(By.XPath("//*[@id=\"edit-field-accepte-communications-wrapper\"]/div")).Click();

    // Retourne le bouton submit du formulaire (sans cliquer — utilisé par les steps When/Then)
    public IWebElement GetSubmitButton()
    {
        var submitBtn = Wait.Until(d => d.FindElement(By.CssSelector(SubmitButton)));
        ScrollIntoView(submitBtn);
        return submitBtn;
    }

    private void AcceptCookiesIfPresent()
    {
        try
        {
            var cookieWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(8));
            cookieWait.Until(d => d.FindElement(By.Id("tarteaucitronPersonalize2")).Displayed);
            Driver.FindElement(By.Id("tarteaucitronPersonalize2")).Click();
            Js.ExecuteScript(
                "var el = document.getElementById('tarteaucitronAlertBig'); if(el) el.remove();");
            Thread.Sleep(1000);
        }
        catch (WebDriverTimeoutException)
        {
            // Pas de bandeau cookies : on continue
        }
    }
}
