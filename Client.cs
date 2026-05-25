namespace FormRegister;

/// <summary>
/// Modèle des données lues depuis testData/*.json.
/// Les propriétés *Value (ex. studyFieldValue) sont les data-value Selectize sur le site.
/// </summary>
public class Client
{
    public string Profile { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string Civility { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string CountryOfResidence { get; set; } = default!;
    public string CountryOfResidenceValue { get; set; } = default!;
    public string Nationality { get; set; } = default!;
    public string PostCode { get; set; } = default!;
    public string City { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Function { get; set; } = default!;
    public string OrganizationType { get; set; } = default!;
    public string OrganizationTypeValue { get; set; } = default!;
    public string OrganizationName { get; set; } = default!;
    public string StudyField { get; set; } = default!;
    public string studyFieldValue { get; set; } = default!;
    public string StudyLevel { get; set; } = default!;
    public string studyLevelValue { get; set; } = default!;
}
