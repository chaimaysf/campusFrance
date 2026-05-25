namespace FormRegister.Config;

/// <summary>
/// URLs centralisées : un seul endroit à modifier si l'URL de test change.
/// </summary>
public static class TestUrls
{
    /// <summary>
    /// Page d'inscription. Surcharge possible via variable d'environnement
    /// CAMPUS_FRANCE_INSCRIPTION_URL (utile en CI ou autre environnement).
    /// </summary>
    public static string Inscription =>
        Environment.GetEnvironmentVariable("CAMPUS_FRANCE_INSCRIPTION_URL")
        ?? "https://www.campusfrance.org/fr/user/register";
}
