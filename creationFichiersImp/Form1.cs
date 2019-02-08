using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Odbc;

namespace creationFichiersImp
{
    public partial class FormCreateImp : Form
    {
        gestionIni monFicIni = new gestionIni();
        logApp monFicLog = new logApp();
        ParseCsv ficAparser = new ParseCsv();
        FichierImp ficImpAcreer = new FichierImp();
        Boolean mesTests = true;
        Boolean succesParse = false;
        String leFichier;
        String nomFichierCSV;
        String repFichierCSV;
        String portServeur;
        String adrServeur;
        String nomBase;
        String loginPG = "postgres";
        String passPG = "pgpass";
        String requetes;
        String paramConnexion;
        String repApplication = Application.StartupPath;
        
        String cheminMAJDonnees = null;
        private OdbcConnection chaineConnexionListeBases = new OdbcConnection();
        private OdbcCommand mesRequetes;
        private OdbcDataReader retourRequetes;
        int nbLigneFicCsv = 0;
        String maintenant = DateTime.Now.ToString("yyMMdd_HHmmss");



        public FormCreateImp()
        {
            InitializeComponent();
        }


        private void FormCreateImp_Load(object sender, EventArgs e)
        {
            // Initialisation des variables de configuration
            monFicIni.Filename = Application.StartupPath + "\\params.ini";

            //LOGS
            String repLog = monFicIni.ReadString("LOG", "RepLog");
            String ficLog = repLog + "log_" + maintenant + ".txt";

            string[] arguments = Environment.GetCommandLineArgs();

            if (arguments.Length == 1)
            {

                monFicLog.Log("Ouverture de l'outil.", ficLog);

                nomFichierCSV = monFicIni.ReadString("FIC", "NomFic");
                repFichierCSV = monFicIni.ReadString("FIC", "RepFic");
                textBoxFicCSV.Text = repFichierCSV + nomFichierCSV;

                portServeur = monFicIni.ReadString("BDD", "PortServeur");
                adrServeur = monFicIni.ReadString("BDD", "AdrServeur");
                nomBase = monFicIni.ReadString("BDD", "NomBase");


                // Initialisation des variables
                mesTests = true;

                // Initialisation de l'objet représentant le fichier CSV à parser
                succesParse = ficAparser.TraitementCsvToObj(textBoxFicCSV.Text);

                if (succesParse)
                {
                    monFicLog.Log("Lecture du fichier CSV = OK.", ficLog);
                }
                else
                {
                    monFicLog.Log("Fichier CSV absent de la configuration ou défaut à sa lecture.", ficLog);
                }

                // Initialisations des paramètres pour le passage de la requête
                requetes = "SELECT datname FROM pg_database WHERE datistemplate=FALSE and datname not like 'postgres'";
                paramConnexion = "Driver={PostgreSQL Unicode};Server=" + adrServeur + ";Port=" + portServeur + ";Database=postgres;Uid=" + loginPG + ";Pwd=" + passPG + ";";

                // Remplissage des textboxs
                if (portServeur == null)
                {
                    textBoxPort.Text = "5432";
                }
                else
                {
                    textBoxPort.Text = portServeur;
                }

                if (adrServeur == null)
                {
                    textBoxServeur.Text = "127.0.0.1";
                }
                else
                {
                    textBoxServeur.Text = adrServeur;
                }

                // Remplissage du comboBox
                // Connexion à la base
                using (chaineConnexionListeBases = new OdbcConnection(paramConnexion))
                {
                    try
                    {
                        // Passage de la commande
                        mesRequetes = new OdbcCommand(requetes, chaineConnexionListeBases);
                        // Ouverture
                        chaineConnexionListeBases.Open();
                    }
                    catch (Exception ex)
                    {
                        monFicLog.Log("Erreur à la connexion ODBC pour la récupération des bases.", ficLog);
                        monFicLog.Log(ex.ToString(), ficLog);
                        mesTests = false;
                    }
                    finally
                    {
                        // Exécution si pas d'exception
                        if (mesTests)
                        {
                            // On met à jour le fichier ini.
                            monFicIni.WriteString("BDD", "PortServeur", textBoxPort.Text);
                            monFicIni.WriteString("BDD", "AdrServeur", textBoxServeur.Text);

                            // Exeécution de la requête
                            retourRequetes = mesRequetes.ExecuteReader();

                            int i = 0;

                            // Remplissage du comboBox
                            while (retourRequetes.Read())
                            {
                                comboBoxBase.Items.Add(retourRequetes["datname"]);
                                i++;
                            }

                            if (i != 0)
                            {
                                monFicLog.Log("Récupération des bases = OK.", ficLog);
                            }

                            // Fermeture de la connexion
                            retourRequetes.Close();

                            // Si un nom de base est déjà présent dans le fichier ini et listé dans le comboBox, on l'affiche, sinon on affiche la première base
                            if ((nomBase != null) && (comboBoxBase.Items.IndexOf(nomBase) != -1))
                            {
                                comboBoxBase.Text = nomBase;
                            }
                            else
                            {
                                comboBoxBase.Text = comboBoxBase.Items[0].ToString();
                            }
                        }
                    }
                }
            }
            else
            {
                if (arguments[1] == "-auto")
                {
                    nomFichierCSV = monFicIni.ReadString("FIC", "NomFic");
                    repFichierCSV = monFicIni.ReadString("FIC", "RepFic");
                    textBoxFicCSV.Text = repFichierCSV + nomFichierCSV;

                    portServeur = monFicIni.ReadString("BDD", "PortServeur");
                    adrServeur = monFicIni.ReadString("BDD", "AdrServeur");
                    nomBase = monFicIni.ReadString("BDD", "NomBase");

                    // Initialisations des paramètres pour le passage de la requête
                    requetes = "SELECT param_val FROM params WHERE param_nom like 'RepMAJDonnees'";
                    paramConnexion = "Driver={PostgreSQL UNICODE};Server=" + adrServeur + ";Port=" + portServeur + ";Database=" + nomBase + ";Uid=" + loginPG + ";Pwd=" + passPG + ";";

                    monFicLog.Log("Démarrage automatique du programme.", ficLog);

                    // Initialisation des variables
                    mesTests = true;
                    string retourCreation = null;

                    // Initialisation de l'objet représentant le fichier CSV à parser
                    succesParse = ficAparser.TraitementCsvToObj(textBoxFicCSV.Text);

                    if (succesParse)
                    {
                        monFicLog.Log("Lecture du fichier CSV = OK.", ficLog);
                    }
                    else
                    {
                        monFicLog.Log("Fichier CSV absent de la configuration ou défaut à la lecture.", ficLog);
                    }


                    using (chaineConnexionListeBases = new OdbcConnection(paramConnexion))
                    {
                        try
                        {
                            // Passage de la commande
                            mesRequetes = new OdbcCommand(requetes, chaineConnexionListeBases);
                            // Ouverture
                            chaineConnexionListeBases.Open();
                        }
                        catch (Exception ex)
                        {
                            monFicLog.Log("Erreur à la connexion ODBC pour la récupération des bases.", ficLog);
                            monFicLog.Log(ex.ToString(), ficLog);
                            mesTests = false;
                            MessageBox.Show("Erreur à la connexion à la base sélectionnée.");
                        }
                        finally
                        {
                            // Exécution si pas d'exception
                            if (mesTests)
                            {
                                // Exeécution de la requête
                                retourRequetes = mesRequetes.ExecuteReader();

                                // Remplissage du comboBox
                                while (retourRequetes.Read())
                                {
                                    cheminMAJDonnees = retourRequetes["param_val"].ToString();
                                }

                                monFicLog.Log("Répertoire trouvé pour la création des fichiers *.imp : " + cheminMAJDonnees + "IMP", ficLog);

                                // Fermeture de la connexion
                                retourRequetes.Close();

                                ficImpAcreer.CreateDico(monFicIni);

                                nbLigneFicCsv = ficAparser.GetLines().Count;
                                for (int i = 0; i < nbLigneFicCsv; i++)
                                {
                                    retourCreation = ficImpAcreer.CreateFile(cheminMAJDonnees, ficAparser.GetHeader(), ficAparser.GetLines().ElementAt(i), ficLog, monFicLog);
                                    if (retourCreation == "-1")
                                    {
                                        monFicLog.Log("Code client absent de l'en-tête du fichier CSV.", ficLog);
                                        break;
                                    }
                                    else
                                    {
                                        monFicLog.Log(retourCreation, ficLog);
                                    }
                                    
                                }
                            }

                            Application.Exit();
                        }
                    }

                }
            }
        }


        private void textBoxServeur_KeyUp(object sender, KeyEventArgs e)
        {
            // Initialisation des variables de configuration
            monFicIni.Filename = Application.StartupPath + "\\params.ini";

            //LOGS
            String repLog = monFicIni.ReadString("LOG", "Replog");
            String ficLog = repLog + "log_" + maintenant + ".txt";

            if ((e.KeyCode == Keys.Return) && (textBoxPort.Text != null))
            {
                // Initialisation des variables
                mesTests = true;

                // Récupération des valeurs en textBoxs
                portServeur = textBoxPort.Text;
                adrServeur = textBoxServeur.Text;

                // Initialisations des paramètres pour le passage de la requête
                requetes = "SELECT datname FROM pg_database WHERE datistemplate=FALSE and datname not like 'postgres'";
                paramConnexion = "Driver={PostgreSQL Unicode};Server=" + adrServeur + ";Port=" + portServeur + ";Database=postgres;Uid=" + loginPG + ";Pwd=" + passPG + ";";

                // Si le port et l'adresse sont bien renseignés
                // Mise à jour du comboBox
                // Connexion à la base
                if ((portServeur != null) && (adrServeur != null))
                {
                    using (chaineConnexionListeBases = new OdbcConnection(paramConnexion))
                    {
                        try
                        {
                            // Passage de la commande
                            mesRequetes = new OdbcCommand(requetes, chaineConnexionListeBases);
                            // Ouverture
                            chaineConnexionListeBases.Open();
                        }
                        catch (Exception ex)
                        {
                            monFicLog.Log("Erreur à la connexion ODBC pour la récupération des bases.", ficLog);
                            monFicLog.Log(ex.ToString(), ficLog);
                            mesTests = false;
                            MessageBox.Show("Erreur à la connexion à la base, vérifiez les paramètres.");
                        }
                        finally
                        {
                            // Exécution si pas d'exception
                            if (mesTests)
                            {
                                // Mise à jour du fichier ini
                                monFicIni.WriteString("BDD", "PortServeur", textBoxPort.Text);
                                monFicIni.WriteString("BDD", "AdrServeur", textBoxServeur.Text);

                                // Exeécution de la requête
                                retourRequetes = mesRequetes.ExecuteReader();

                                // On vide la comboBox
                                comboBoxBase.Items.Clear();

                                // Remplissage du comboBox
                                while (retourRequetes.Read())
                                {
                                    comboBoxBase.Items.Add(retourRequetes["datname"]);
                                }

                                // Fermeture de la connexion
                                retourRequetes.Close();

                                // On récupère la première valeur
                                comboBoxBase.Text = comboBoxBase.Items[0].ToString();
                            }
                        }
                    }
                }
            }
        }

        private void textBoxPort_KeyUp(object sender, KeyEventArgs e)
        {
            // Initialisation des variables de configuration
            monFicIni.Filename = Application.StartupPath + "\\params.ini";

            //LOGS
            String repLog = monFicIni.ReadString("LOG", "RepLog");
            String ficLog = repLog + "log_" + maintenant + ".txt";

            if ((e.KeyCode == Keys.Return) && (textBoxServeur.Text != null))
            {
                // Initialisation des variables
                mesTests = true;

                // Récupération des valeurs en textBoxs
                portServeur = textBoxPort.Text;
                adrServeur = textBoxServeur.Text;

                // Initialisations des paramètres pour le passage de la requête
                requetes = "SELECT datname FROM pg_database WHERE datistemplate=FALSE and datname not like 'postgres'";
                paramConnexion = "Driver={PostgreSQL UNICODE};Server=" + adrServeur + ";Port=" + portServeur + ";Database=postgres;Uid=" + loginPG + ";Pwd=" + passPG + ";";

                // Si le port et l'adresse sont bien renseignés
                // Mise à jour du comboBox
                // Connexion à la base
                if ((portServeur != null) && (adrServeur != null))
                {
                    using (chaineConnexionListeBases = new OdbcConnection(paramConnexion))
                    {
                        try
                        {
                            // Passage de la commande
                            mesRequetes = new OdbcCommand(requetes, chaineConnexionListeBases);
                            // Ouverture
                            chaineConnexionListeBases.Open();
                        }
                        catch (Exception ex)
                        {
                            monFicLog.Log("Erreur à la connexion ODBC pour la récupération des bases.", ficLog);
                            monFicLog.Log(ex.ToString(), ficLog);
                            mesTests = false;
                            MessageBox.Show("Erreur à la connexion à la base, vérifiez les paramètres.");
                        }
                        finally
                        {
                            // Exécution si pas d'exception
                            if (mesTests)
                            {
                                // Mise à jour du fichier ini
                                monFicIni.WriteString("BDD", "PortServeur", textBoxPort.Text);
                                monFicIni.WriteString("BDD", "AdrServeur", textBoxServeur.Text);

                                // Exeécution de la requête
                                retourRequetes = mesRequetes.ExecuteReader();

                                // On vide la comboBox
                                comboBoxBase.Items.Clear();

                                // Remplissage du comboBox
                                while (retourRequetes.Read())
                                {
                                    comboBoxBase.Items.Add(retourRequetes["datname"]);
                                }

                                // Fermeture de la connexion
                                retourRequetes.Close();

                                // On récupère la première valeur
                                comboBoxBase.Text = comboBoxBase.Items[0].ToString();
                            }
                        }
                    }
                }
            }
        }

        private void comboBoxBase_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Initialisation des variables de configuration
            monFicIni.Filename = Application.StartupPath + "\\params.ini";

            //LOGS
            String repLog = monFicIni.ReadString("LOG", "RepLog");
            String ficLog = repLog + "log_" + maintenant + ".txt";

            monFicIni.WriteString("BDD", "NomBase", comboBoxBase.Text);
        }

        private void buttonChoixFichier_Click(object sender, EventArgs e)
        {
            // Initialisation des variables de configuration
            monFicIni.Filename = Application.StartupPath + "\\params.ini";

            //LOGS
            String repLog = monFicIni.ReadString("LOG", "Replog");
            String ficLog = repLog + "log_" + maintenant + ".txt";

            if (openFileDialogCSV.ShowDialog() == DialogResult.OK)
            {
                leFichier = openFileDialogCSV.FileName;
                textBoxFicCSV.Text = leFichier;
                
                nomFichierCSV = leFichier.Substring(leFichier.LastIndexOf("\\") + 1);
                repFichierCSV = leFichier.Substring(0, leFichier.LastIndexOf("\\") + 1);

                monFicIni.WriteString("FIC", "RepFic", repFichierCSV);
                monFicIni.WriteString("FIC", "NomFic", nomFichierCSV);


                succesParse = ficAparser.TraitementCsvToObj(leFichier);

                if (succesParse)
                {
                    monFicLog.Log("Lecture du fichier CSV = OK.", ficLog);
                }
                else
                {
                    monFicLog.Log("Fichier CSV absent de la configuration ou défaut à sa lecture.", ficLog);
                }

            }
        }

        private void buttonConvert_Click(object sender, EventArgs e)
        {
            // Initialisation des variables de configuration
            monFicIni.Filename = Application.StartupPath + "\\params.ini";

            //LOGS
            String repLog = monFicIni.ReadString("LOG", "RepLog");
            String ficLog = repLog + "log_" + maintenant + ".txt";

            // Initialisations des paramètres pour le passage de la requête
            requetes = "SELECT param_val FROM params WHERE param_nom like 'RepMAJDonnees'";
            paramConnexion = "Driver={PostgreSQL UNICODE};Server=" + adrServeur + ";Port=" + portServeur + ";Database=" + comboBoxBase.Text +";Uid=" + loginPG + ";Pwd=" + passPG + ";";
            string retourCreation = null;

            using (chaineConnexionListeBases = new OdbcConnection(paramConnexion))
            {
                try
                {
                    // Passage de la commande
                    mesRequetes = new OdbcCommand(requetes, chaineConnexionListeBases);
                    // Ouverture
                    chaineConnexionListeBases.Open();
                }
                catch (Exception ex)
                {
                    monFicLog.Log("Erreur à la connexion ODBC pour la récupération des bases.", ficLog);
                    monFicLog.Log(ex.ToString(), ficLog);
                    mesTests = false;
                    MessageBox.Show("Erreur à la connexion à la base sélectionnée.");
                }
                finally
                {
                    // Exécution si pas d'exception
                    if (mesTests)
                    {
                        // Exécution de la requête
                        retourRequetes = mesRequetes.ExecuteReader();

                        // Tant qu'il y a des données retournées par la requête
                        while (retourRequetes.Read())
                        {
                            cheminMAJDonnees = retourRequetes["param_val"].ToString();
                        }

                        monFicLog.Log("Répertoire trouvé pour la création des fichiers *.imp : " + cheminMAJDonnees + "IMP", ficLog);

                        // Fermeture de la connexion
                        retourRequetes.Close();
                        monFicLog.Log("Données récupérées pour le traitement, fermeture de la connexion à la base de données.", ficLog);

                        // Association des champs déclarés dans le fichier ini aux en-têtes présentent dans le fichier CSV 
                        monFicLog.Log("Début de lecture du fichier *.ini pour l'association aux en-têtes présentent dans le fichier *.csv.", ficLog);
                        ficImpAcreer.CreateDico(monFicIni);
                        monFicLog.Log("Fin de lecture du fichier *.ini pour l'association aux en-têtes présentent dans le fichier *.csv.", ficLog);

                        // Combien de lignes à traiter
                        monFicLog.Log("Traitement pour connaître le nombre de lignes à traiter.", ficLog);
                        nbLigneFicCsv = ficAparser.GetLines().Count;
                        monFicLog.Log("Nombre de lignes à traiter = " + nbLigneFicCsv + ".", ficLog);
                        for (int i = 0;i<nbLigneFicCsv;i++)
                        {
                            retourCreation = ficImpAcreer.CreateFile(cheminMAJDonnees, ficAparser.GetHeader(), ficAparser.GetLines().ElementAt(i), ficLog, monFicLog);
                            monFicLog.Log("Traitement de la ligne " + i + " terminé.", ficLog);
                            if (retourCreation == "-1")
                            {
                                monFicLog.Log("Code client absent de l'en-tête du fichier CSV.", ficLog);
                                break;
                            }
                            else
                            {
                                monFicLog.Log(retourCreation, ficLog);
                            }
                        }                      
                    }
                }
            }
        }
    }
}
