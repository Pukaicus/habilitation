using habilitations2024.model;
using System;
using System.Collections.Generic;

namespace habilitations2024.dal
{
    /// <summary>
    /// Classe permettant de gérer les demandes concernant les développeurs
    /// </summary>
    public class DeveloppeurAccess
    {
        /// <summary>
        /// Instance unique de l'accès aux données
        /// </summary>
        private readonly Access access = null;

        /// <summary>
        /// Constructeur pour créer l'accès aux données
        /// </summary>
        public DeveloppeurAccess()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// Controle si l'utilisateur a le droit de se connecter (nom, prénom, pwd et profil "admin")
        /// </summary>
        /// <param name="admin"></param>
        /// <returns>vrai si l'utilisateur a le profil "admin"</returns>
        public bool ControleAuthentification(Admin admin)
        {
            if (access.Manager != null)
            {
                string req = "select * from developpeur d join profil p on d.idprofil = p.idprofil " +
                             "where d.nom = @nom and d.prenom = @prenom and pwd = SHA2(@pwd, 256) and p.nom = 'admin';";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@nom", admin.Nom },
                    { "@prenom", admin.Prenom },
                    { "@pwd", admin.Pwd }
                };
                try
                {
                    List<object[]> records = access.Manager.ReqSelect(req, parameters);
                    return records != null && records.Count > 0;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Environment.Exit(0);
                }
            }
            return false;
        }

        /// <summary>
        /// Récupère et retourne les développeurs sans filtre (ancienne version)
        /// </summary>
        /// <returns>liste des développeurs</returns>
        public List<Developpeur> GetLesDeveloppeursSansFiltre()
        {
            List<Developpeur> lesDeveloppeurs = new List<Developpeur>();
            if (access.Manager != null)
            {
                string req = "select d.iddeveloppeur, d.nom, d.prenom, d.tel, d.mail, p.idprofil, p.nom " +
                             "from developpeur d join profil p on d.idprofil = p.idprofil " +
                             "order by d.nom, d.prenom;";
                try
                {
                    List<object[]> records = access.Manager.ReqSelect(req);
                    if (records != null)
                    {
                        foreach (object[] record in records)
                        {
                            Profil profil = new Profil((int)record[5], (string)record[6]);
                            Developpeur developpeur = new Developpeur((int)record[0], (string)record[1], (string)record[2],
                                (string)record[3], (string)record[4], profil);
                            lesDeveloppeurs.Add(developpeur);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Environment.Exit(0);
                }
            }
            return lesDeveloppeurs;
        }

        /// <summary>
        /// Récupère et retourne les développeurs avec filtre optionnel sur profil
        /// </summary>
        /// <param name="profil">Filtre sur le nom du profil</param>
        /// <returns>liste des développeurs filtrés</returns>
        public List<Developpeur> GetLesDeveloppeurs(string profil = null)
        {
            List<Developpeur> lesDeveloppeurs = new List<Developpeur>();
            if (access.Manager != null)
            {
                string req = "select d.iddeveloppeur, d.nom, d.prenom, d.tel, d.mail, p.idprofil, p.nom " +
                             "from developpeur d join profil p on d.idprofil = p.idprofil ";
                Dictionary<string, object> parameters = new Dictionary<string, object>();

                if (!string.IsNullOrEmpty(profil))
                {
                    req += "where p.nom = @profil ";
                    parameters.Add("@profil", profil);
                }

                req += "order by d.nom, d.prenom;";

                try
                {
                    List<object[]> records = access.Manager.ReqSelect(req, parameters);
                    if (records != null)
                    {
                        foreach (object[] record in records)
                        {
                            Profil profilObj = new Profil((int)record[5], (string)record[6]);
                            Developpeur developpeur = new Developpeur((int)record[0], (string)record[1], (string)record[2],
                                (string)record[3], (string)record[4], profilObj);
                            lesDeveloppeurs.Add(developpeur);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Environment.Exit(0);
                }
            }
            return lesDeveloppeurs;
        }
public class DeveloppeurAccess
{
    // Exemple simple, ta vraie méthode va interagir avec ta BDD
    private List<Developpeur> listeDeveloppeurs = new List<Developpeur>()
    {
        new Developpeur{ Id=1, Nom="Dupont", Profil="Frontend"},
        new Developpeur{ Id=2, Nom="Martin", Profil="Backend"},
        new Developpeur{ Id=3, Nom="Durand", Profil="Frontend"},
        new Developpeur{ Id=4, Nom="Lemoine", Profil="Fullstack"}
    };

    // Nouvelle méthode modifiée
    public List<Developpeur> GetLesDeveloppeurs(string profil = "")
    {
        if (string.IsNullOrEmpty(profil))
            return listeDeveloppeurs; // Tous les devs
        else
            return listeDeveloppeurs.Where(d => d.Profil == profil).ToList();
    }
}

public class Developpeur
{
    public int Id { get; set; }
    public string Nom { get; set; }
    public string Profil { get; set; }
}


        /// <summary>
        /// Demande de suppression d'un développeur
        /// </summary>
        /// <param name="developpeur">objet developpeur à supprimer</param>
        public void DelDepveloppeur(Developpeur developpeur)
        {
            if (access.Manager != null)
            {
                string req = "delete from developpeur where iddeveloppeur = @iddeveloppeur;";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@iddeveloppeur", developpeur.Iddeveloppeur }
                };
                try
                {
                    access.Manager.ReqUpdate(req, parameters);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Environment.Exit(0);
                }
            }
        }

        /// <summary>
        /// Demande d'ajout un développeur
        /// </summary>
        /// <param name="developpeur">objet developpeur à ajouter</param>
        public void AddDeveloppeur(Developpeur developpeur)
        {
            if (access.Manager != null)
            {
                string req = "insert into developpeur(nom, prenom, tel, mail, pwd, idprofil) " +
                             "values (@nom, @prenom, @tel, @mail, SHA2(@pwd, 256), @idprofil);";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@nom", developpeur.Nom },
                    { "@prenom", developpeur.Prenom },
                    { "@tel", developpeur.Tel },
                    { "@mail", developpeur.Mail },
                    { "@pwd", developpeur.Pwd },      // Correction : tu avais developpeur.Nom ici, doit être developpeur.Pwd
                    { "@idprofil", developpeur.Profil.Idprofil }
                };
                try
                {
                    access.Manager.ReqUpdate(req, parameters);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Environment.Exit(0);
                }
            }
        }

        /// <summary>
        /// Demande de modification d'un développeur
        /// </summary>
        /// <param name="developpeur">objet developpeur à modifier</param>
        public void UpdateDeveloppeur(Developpeur developpeur)
        {
            if (access.Manager != null)
            {
                string req = "update developpeur set nom = @nom, prenom = @prenom, tel = @tel, mail = @mail, idprofil = @idprofil " +
                             "where iddeveloppeur = @iddeveloppeur;";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@iddeveloppeur", developpeur.Iddeveloppeur },  // attention majuscules cohérentes
                    { "@nom", developpeur.Nom },
                    { "@prenom", developpeur.Prenom },
                    { "@tel", developpeur.Tel },
                    { "@mail", developpeur.Mail },
                    { "@idprofil", developpeur.Profil.Idprofil }
                };
                try
                {
                    access.Manager.ReqUpdate(req, parameters);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Environment.Exit(0);
                }
            }
        }

        /// <summary>
        /// Demande de modification du mot de passe
        /// </summary>
        /// <param name="developpeur">objet developpeur avec nouveau pwd</param>
        public void UpdatePwd(Developpeur developpeur)
        {
            if (access.Manager != null)
            {
                string req = "update developpeur set pwd = SHA2(@pwd, 256) where iddeveloppeur = @iddeveloppeur;";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@iddeveloppeur", developpeur.Iddeveloppeur },
                    { "@pwd", developpeur.Pwd }
                };
                try
                {
                    access.Manager.ReqUpdate(req, parameters);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Environment.Exit(0);
                }
            }
        }
    }
}
