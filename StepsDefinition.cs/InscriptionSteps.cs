// =============================================================================
// Step definitions Reqnroll : relie les phrases du .feature au code C#
// - Pas de sélecteurs Selenium ici (délégués à InscriptionPage = POM)
// - Les données viennent des JSON via ClientReader
// =============================================================================

using FormRegister.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;

namespace FormRegister;

[Binding]
public class StepDefinitions
{
    private readonly ScenarioContext _scenarioContext;
    private IWebDriver? _driver;
    private InscriptionPage? _inscriptionPage;

    // Tous les profils chargés au début du scénario (student, searcher, admin JSON)
    private List<Client> _clients = new();

    public StepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    // Avant chaque scénario : démarre Chrome, crée la Page Object, charge les JSON
    [BeforeScenario]
    public void BeforeScenario()
    {
        _driver = new ChromeDriver();
        _driver.Manage().Window.Maximize();
        _scenarioContext.Set(_driver, "driver");
        _inscriptionPage = new InscriptionPage(_driver);
        _clients = ClientReader.ReadDataFromJson();
    }

    // Après chaque scénario : ferme le navigateur
    [AfterScenario]
    public void AfterScenario()
    {
        _driver?.Quit();
    }

    // Given I am on the "Registration" page
    [Given(@"I am on the ""(.*)"" page")]
    public void OpenPage(string pageName)
    {
        if (pageName != "Registration")
            throw new ArgumentException($"Unknown page: {pageName}");

        _inscriptionPage!.Open();
    }

    // student → testData/student.json (Profile = "Etudiant" dans le fichier)
    [Given(@"I fill the registration form as a student")]
    public void FillStudentForm() =>
        _inscriptionPage!.FillStudentProfile(_clients.First(c => c.Profile == "Etudiant"));

    // searcher → testData/searcher.json (Profile = "Chercheur")
    [Given(@"I fill the registration form as a searcher")]
    public void FillSearcherForm() =>
        _inscriptionPage!.FillSearcherProfile(_clients.First(c => c.Profile == "Chercheur"));

    // institutional → testData/admin.json (Profile = "Institutionnel")
    [Given(@"I fill the registration form as an institutional user")]
    public void FillInstitutionnelForm() =>
        _inscriptionPage!.FillInstitutionnelProfile(_clients.First(c => c.Profile == "Institutionnel"));

    // When : vérifie le bouton dans #user-form (évite le bouton "Je me connecte" du header)
    // Pas de clic — conforme au commentaire dans le .feature
    [When(@"I verify the registration submit button is available")]
    public void VerifySubmitButtonAvailable()
    {
        var submitBtn = _inscriptionPage!.GetSubmitButton();
        Assert.That(submitBtn.Displayed, Is.True);
        Assert.That(submitBtn.GetAttribute("value"), Does.Contain("un compte").IgnoreCase);
    }

    // Then : le libellé du bouton doit contenir le texte du scénario (ex. "Créer un compte")
    // C'est le texte affiché sur le site français, pas une traduction du .feature
    [Then(@"the submit button should display ""(.*)""")]
    public void SubmitButtonShouldDisplay(string expectedLabel)
    {
        var submitBtn = _inscriptionPage!.GetSubmitButton();
        Assert.That(submitBtn.GetAttribute("value"), Does.Contain(expectedLabel));
    }
}
