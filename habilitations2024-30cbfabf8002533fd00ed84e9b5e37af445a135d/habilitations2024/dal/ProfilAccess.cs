using habilitations2024.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace habilitations2024.dal
{
    /// <summary>
    /// Classe permettant de gérer les demandes concernant les profils
    /// </summary>
    public class ProfilAccess
    {
        /// <summary>
        /// Instance unique de l'accès aux données
        /// </summary>
        private readonly Access access = null;

        /// <summary>
        /// Constructeur pour créer l'accès aux données
        /// </summary>
        public ProfilAccess()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// Récupère et retourne les profils
        /// </summary>
        /// <returns>liste des profils</returns>
        public List<Profil> GetLesProfils()
        {
            List<Profil> lesProfils = new List<Profil>();
            if (access.Manager != null)
            {
                string req = "select * from profil order by nom;";
                try
                {
                    List<Object[]> records = access.Manager.ReqSelect(req);
                    if (records != null)
                    {
                        foreach (Object[] record in records)
                        {
                            Profil profil = new Profil((int)record[0], (string)record[1]);
                            lesProfils.Add(profil);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Environment.Exit(0);
                }
            }
            return lesProfils;
        }

        // ===== AJOUT de la méthode 2, telle quelle, sous la méthode existante =====

        public static List<Profil> GetLesProfils()
        {
            string sql = "SELECT * FROM profil";
            List<Object[]> records = BddManager.GetInstance(ConfigurationManager.ConnectionStrings["HabilitationsDB"].ConnectionString)
                                              .ReqSelect(sql);

            List<Profil> lesProfils = new List<Profil>();
            foreach (Object[] row in records)
            {
                lesProfils.Add(new Profil((int)row[0], (string)row[1]));
            }
            return lesProfils;
        }

    }
}
