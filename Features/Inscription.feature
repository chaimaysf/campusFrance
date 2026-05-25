# =============================================================================
# Fichier Gherkin : scénarios lisibles par tout le monde (métier + technique)
# Lié au code : StepsDefinition.cs/InscriptionSteps.cs
# Données de test : fichiers JSON dans testData/ (voir liens ci-dessous)
# =============================================================================

# Tag sur toute la feature : lancer uniquement ces tests avec
#   dotnet test --filter "Category=inscription"
@inscription
Feature: Registration on Campus France
  As a Campus France platform user
  I want to complete the account registration form
  So that I can verify the form is filled and ready to submit

  # Les tests ne cliquent PAS sur le bouton (pas d'inscription réelle sur le site).
  # On vérifie seulement que le formulaire est rempli et que le bouton est correct.

  # Exécuté avant CHAQUE scénario : ouvre la page d'inscription + cookies
  Background:
    Given I am on the "Registration" page

  # --- Profil Étudiant ---
  # Données : testData/student.json  (profile = "Etudiant" dans le JSON)
  # Tag : dotnet test --filter "Category=student"
  @student
  Scenario: Student profile - submit button is ready
    Given I fill the registration form as a student
    When I verify the registration submit button is available
    Then the submit button should display "Créer un compte"

  # --- Profil Chercheur ---
  # Données : testData/searcher.json  (profile = "Chercheur")
  # Tag : dotnet test --filter "Category=searcher"
  @searcher
  Scenario: Searcher profile - submit button is ready
    Given I fill the registration form as a searcher
    When I verify the registration submit button is available
    Then the submit button should display "Créer un compte"

  # --- Profil Institutionnel ---
  # Données : testData/admin.json  (profile = "Institutionnel")
  # Tag : dotnet test --filter "Category=institutional"
  @institutional
  Scenario: Institutional profile - submit button is ready
    Given I fill the registration form as an institutional user
    When I verify the registration submit button is available
    Then the submit button should display "Créer un compte"
