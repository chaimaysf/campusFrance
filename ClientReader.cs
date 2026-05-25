using System.Text.Json;

namespace FormRegister;

/// <summary>
/// Charge les données de test depuis les fichiers JSON du dossier testData/.
/// Chaque scénario du .feature utilise un profil (Etudiant, Chercheur, Institutionnel).
/// </summary>
public class ClientReader
{
    public static List<Client> ReadDataFromJson()
    {
        // Un fichier JSON par profil — copiés dans bin/Debug/.../testData à la compilation
        string[] forms =
        {
            "testData/admin.json",      // profil Institutionnel
            "testData/searcher.json",   // profil Chercheur
            "testData/student.json"     // profil Etudiant
        };

        var clients = new List<Client>();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        foreach (string path in forms)
        {
            string json = File.ReadAllText(path);
            Client? client = JsonSerializer.Deserialize<Client>(json, options);
            if (client != null)
                clients.Add(client);
        }

        return clients;
    }
}
