// Crée un nouveau projet "Tests" dans ta solution, puis ajoute ce fichier.
[TestClass]
public class DeveloppeurAccessTests
{
    private const string TEST_CONNECTION_STRING = "Server=...;Database=test_habilitations;...";

    [TestMethod]
    public void GetLesDeveloppeurs_FiltreAdmin_RetourneListeFiltree()
    {
        // Arrange
        BddManager.GetInstance(TEST_CONNECTION_STRING).ReqUpdate("INSERT INTO profil (libelle) VALUES ('admin')");
        BddManager.GetInstance(TEST_CONNECTION_STRING).ReqUpdate("INSERT INTO developpeur (nom, profil_id) VALUES ('TestAdmin', 1)");

        // Act
        var result = DeveloppeurAccess.GetLesDeveloppeurs("admin");

        // Assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("TestAdmin", result[0].Nom);
    }
}