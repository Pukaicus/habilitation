using habilitations2024.controller;
using habilitations2024.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace habilitations2024.view
{
    /// <summary>
    /// Fenêtre d'affichage des développeurs et de leurs profils
    /// </summary>
    public partial class FrmHabilitations : Form
    {
        /// <summary>
        /// Booléen pour savoir si une modification est demandée
        /// </summary>
        private Boolean enCoursDeModifDeveloppeur = false;

        /// <summary>
        /// Objet pour gérer la liste des développeurs
        /// </summary>
        private BindingSource bdgDeveloppeurs = new BindingSource();

        /// <summary>
        /// Objet pour gérer la liste des profils
        /// </summary>
        private BindingSource bdgProfils = new BindingSource();

        /// <summary>
        /// Controleur de la fenêtre
        /// </summary>
        private readonly FrmHabilitationsController controller;

        public FrmHabilitations()
        {
            InitializeComponent();
            controller = new FrmHabilitationsController();

            Init();
        }

        /// <summary>
        /// Initialisations :
        /// Création du controleur et remplissage des listes
        /// </summary>
        private void Init()
        {
            RemplirComboProfils();
            RemplirListeDeveloppeurs();
            EnCourseModifDeveloppeur(false);
            EnCoursModifPwd(false);
        }

        /// <summary>
        /// Remplit le combo profils avec un filtre possible (ligne vide)
        /// </summary>
        private void RemplirComboProfils()
        {
            cboProfil.Items.Clear();
            cboProfil.Items.Add(""); // Ligne vide pour aucun filtre
            List<Profil> lesProfils = controller.GetLesProfils();
            foreach (Profil p in lesProfils)
            {
                cboProfil.Items.Add(p.Libelle);
            }
            cboProfil.SelectedIndex = 0;
        }

        /// <summary>
        /// Affiche les développeurs, avec possibilité de filtrer par profil
        /// </summary>
        /// <param name="profil">nom du profil pour filtre ou null</param>
        private void RemplirListeDeveloppeurs(string profil = null)
        {
            List<Developpeur> lesDeveloppeurs = controller.GetLesDeveloppeurs(profil);
            bdgDeveloppeurs.DataSource = lesDeveloppeurs;
            dgvDeveloppeurs.DataSource = bdgDeveloppeurs;

            dgvDeveloppeurs.Columns["iddeveloppeur"].Visible = false;
            dgvDeveloppeurs.Columns["pwd"].Visible = false;
            dgvDeveloppeurs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        /// <summary>
        /// Evénement lors d'un changement de sélection dans le combo profil : filtre les développeurs
        /// </summary>
        private void cboProfil_SelectedIndexChanged(object sender, EventArgs e)
        {
            string profilSelectionne = cboProfil.SelectedItem?.ToString();
            if (profilSelectionne == "")
                profilSelectionne = null;
            RemplirListeDeveloppeurs(profilSelectionne);
        }

        /// <summary>
        ///  Demande de modification d'un développeur
        /// </summary>
        private void BtnDemandeModifDev_Click(object sender, EventArgs e)
        {
            if (dgvDeveloppeurs.SelectedRows.Count > 0)
            {
                EnCourseModifDeveloppeur(true);
                Developpeur developpeur = (Developpeur)bdgDeveloppeurs.List[bdgDeveloppeurs.Position];
                txtNom.Text = developpeur.Nom;
                txtPrenom.Text = developpeur.Prenom;
                txtTel.Text = developpeur.Tel;
                txtMail.Text = developpeur.Mail;
                cboProfil.SelectedIndex = cboProfil.FindStringExact(developpeur.Profil.Libelle);
            }
            else
            {
                MessageBox.Show("Une ligne doit être sélectionnée.", "Information");
            }
        }

        /// <summary>
        /// Demande de suppression d'un développeur
        /// </summary>
        private void BtnDemandeSupprDev_Click(object sender, EventArgs e)
        {
            if (dgvDeveloppeurs.SelectedRows.Count > 0)
            {
                Developpeur developpeur = (Developpeur)bdgDeveloppeurs.List[bdgDeveloppeurs.Position];
                if (MessageBox.Show("Voulez-vous vraiment supprimer " + developpeur.Nom + " " + developpeur.Prenom + " ?", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    controller.DelDeveloppeur(developpeur);
                    RemplirListeDeveloppeurs();
                }
            }
            else
            {
                MessageBox.Show("Une ligne doit être sélectionnée.", "Information");
            }
        }

        /// <summary>
        /// Demande de changement du pwd
        /// </summary>
        private void BtnDemandeChangePwd_Click(object sender, EventArgs e)
        {
            if (dgvDeveloppeurs.SelectedRows.Count > 0)
            {
                EnCoursModifPwd(true);
            }
            else
            {
                MessageBox.Show("Une ligne doit être sélectionnée.", "Information");
            }
        }

        /// <summary>
        /// Demande d'enregistrement de l'ajout ou de la modification d'un développeur
        /// </summary>
        private void BtnEnregDev_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNom.Text) &&
                !string.IsNullOrWhiteSpace(txtPrenom.Text) &&
                !string.IsNullOrWhiteSpace(txtTel.Text) &&
                !string.IsNullOrWhiteSpace(txtMail.Text) &&
                cboProfil.SelectedIndex != -1)
            {
                Profil profil = controller.GetLesProfils().FirstOrDefault(p => p.Libelle == cboProfil.SelectedItem?.ToString());
                if (profil == null)
                {
                    MessageBox.Show("Veuillez sélectionner un profil valide.", "Information");
                    return;
                }

                if (enCoursDeModifDeveloppeur)
                {
                    Developpeur developpeur = (Developpeur)bdgDeveloppeurs.List[bdgDeveloppeurs.Position];
                    developpeur.Nom = txtNom.Text;
                    developpeur.Prenom = txtPrenom.Text;
                    developpeur.Tel = txtTel.Text;
                    developpeur.Mail = txtMail.Text;
                    developpeur.Profil = profil;
                    controller.UpdateDeveloppeur(developpeur);
                }
                else
                {
                    Developpeur developpeur = new Developpeur(0, txtNom.Text, txtPrenom.Text, txtTel.Text, txtMail.Text, profil);
                    controller.AddDeveloppeur(developpeur);
                }
                RemplirListeDeveloppeurs();
                EnCourseModifDeveloppeur(false);
            }
            else
            {
                MessageBox.Show("Tous les champs doivent être remplis.", "Information");
            }
        }

        /// <summary>
        /// Annule la demande d'ajout ou de modification d'un développeur
        /// Vide les zones de saisie du développeur
        /// </summary>
        private void BtnAnnulDev_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Voulez-vous vraiment annuler ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                EnCourseModifDeveloppeur(false);
            }
        }

        /// <summary>
        /// Demande d'enregistrement du nouveau pwd
        /// </summary>
        private void BtnEnregPwd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtPwd1.Text) &&
                !string.IsNullOrWhiteSpace(txtPwd2.Text) &&
                txtPwd1.Text == txtPwd2.Text)
            {
                Developpeur developpeur = (Developpeur)bdgDeveloppeurs.List[bdgDeveloppeurs.Position];
                developpeur.Pwd = txtPwd1.Text;
                controller.UpdatePwd(developpeur);
                EnCoursModifPwd(false);
            }
            else
            {
                MessageBox.Show("Les 2 zones doivent être remplies et de contenu identique", "Information");
            }
        }

        /// <summary>
        /// Annulation de demande d'enregistrement d'un nouveau pwd
        /// </summary>
        private void BtnAnnulPwd_Click(object sender, EventArgs e)
        {
            EnCoursModifPwd(false);
        }

        /// <summary>
        /// Modification d'affichage suivant si on est en cours de modif ou d'ajout d'un developpeur
        /// </summary>
        private void EnCourseModifDeveloppeur(Boolean modif)
        {
            enCoursDeModifDeveloppeur = modif;
            grbLesDeveloppeurs.Enabled = !modif;
            if (modif)
            {
                grbDeveloppeur.Text = "Modifier un développeur";
            }
            else
            {
                grbDeveloppeur.Text = "Ajouter un développeur";
                txtNom.Text = "";
                txtPrenom.Text = "";
                txtTel.Text = "";
                txtMail.Text = "";
            }
        }

        /// <summary>
        /// Modification d'affichage suivant si on est ou non en cours de modif du pwd
        /// </summary>
        private void EnCoursModifPwd(Boolean modif)
        {
            grbPwd.Enabled = modif;
            grbLesDeveloppeurs.Enabled = !modif;
            grbDeveloppeur.Enabled = !modif;
            txtPwd1.Text = "";
            txtPwd2.Text = "";
        }
    }
}
