using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace creationFichiersImp
{
    public class FichierImp
    {
        Dictionary<string, string> dicoD = new Dictionary<string, string>();
        gestionIni unFicIni = new gestionIni();
        string[] dicoT;
        int nbElementDicoT = 0;
        int i = 0;

        /// <summary>
        /// Création d'un dictionnaire à partir d'un fichier *.ini comprenant une section DICO
        /// </summary>
        /// <param name="FicIniSelec"></param>

        public void CreateDico (gestionIni FicIniSelec)
        {
            dicoT = FicIniSelec.ReadSection("DICO");
            nbElementDicoT = dicoT.Length;
            i = 0;
            string key = null;
            string valeur = null;

            foreach (string valeurT in dicoT)
            {
                key = dicoT[i].Substring(0, dicoT[i].LastIndexOf("="));
                valeur = dicoT[i].Substring(dicoT[i].LastIndexOf("=") + 1);

                if (dicoD.ContainsKey(key) == false)
                {
                    dicoD.Add(dicoT[i].Substring(0, dicoT[i].LastIndexOf("=")), dicoT[i].Substring(dicoT[i].LastIndexOf("=") + 1));
                    i++;
                }
            }
        }


        /// <summary>
        /// Création d'un fichier *.imp à partir d'une ligne d'en-tête de colonne avec le paramètre colCsv, et d'une ligne de données avec le paramètre lineCsv.
        /// </summary>
        /// <param name="RepImp"></param>
        /// <param name="colCsv"></param>
        /// <param name="lineCsv"></param>
        /// <returns></returns>
        /// 
        public string CreateFile (string RepImp, string[] colCsv, string[] lineCsv)
        {
            string retour = null;
            string[,] positionCol = new string[dicoD.Count, 2];
            int positionCodeCli = -1;
            int positionNomCli = -1;
            int positionPrenomCli = -1;
            int i = 0;
            int position = 0;
            string FicImp = null;
            IFormatProvider culture = new CultureInfo("fr-FR", true);

            // On regarde si le dictionnaire contient bien la valeur référence au code client, obligatoire pour constituer le nom du fichier
            if (dicoD.ContainsValue("cli.code"))
            {
                // Pour chaque paire de valeur trouvée dans le dictionnaire (clé, valeur) on réalise le traitement
                foreach (KeyValuePair<string, string> kvp in dicoD)
                {
                    // Si la valeur correspond justement au code client, au nom ou au prénom, on relève sa position dans le tableau des en-têtes de colonne, ce qui sera utile pour la création du nom du fichier
                    switch (kvp.Value)
                    {
                        case "cli.code":
                            // Si la position du code client est encore initialisé à -1, c'est la première valeur du dictionnaire à rechercher, sinon on vérifie si la position précédente
                            // rapporte une valeur. Si ce n'est pas le cas, on récupère la position de ce nouvel élément.
                            if (positionCodeCli == -1)
                            {
                                positionCodeCli = Array.IndexOf(colCsv, kvp.Key);
                            }
                            else
                            {
                                if (lineCsv[positionCodeCli] == null)
                                {
                                    positionCodeCli = Array.IndexOf(colCsv, kvp.Key);
                                }
                            }
                            break;
                        case "cli.nom":
                            // Si la position du nom du client est encore initialisé à -1, c'est la première valeur du dictionnaire à rechercher, sinon on vérifie si la position précédente
                            // rapporte une valeur. Si ce n'est pas le cas, on récupère la position de ce nouvel élément.
                            if (positionNomCli == -1)
                            {
                                positionNomCli = Array.IndexOf(colCsv, kvp.Key);
                            }
                            else
                            {
                                if (lineCsv[positionNomCli] == null)
                                {
                                    positionNomCli = Array.IndexOf(colCsv, kvp.Key);
                                }
                            }
                            break;
                        case "cli.prenom":
                            // Si la position du prénom du client est encore initialisé à -1, c'est la première valeur du dictionnaire à rechercher, sinon on vérifie si la position précédente
                            // rapporte une valeur. Si ce n'est pas le cas, on récupère la position de ce nouvel élément.
                            if (positionPrenomCli == -1)
                            {
                                positionPrenomCli = Array.IndexOf(colCsv, kvp.Key);
                            }
                            else
                            {
                                if (lineCsv[positionPrenomCli] == null)
                                {
                                    positionPrenomCli = Array.IndexOf(colCsv, kvp.Key);
                                }
                            }
                            break;
                        default:
                            break;
                    }

                    // Puis on créé un tableau avec les valeurs du dictionnaire et la position des clés dans le tableau d'en-tête du fichier CSV
                    if (i==0)
                    {
                        positionCol[i, 0] = kvp.Value;
                        positionCol[i, 1] = Array.IndexOf(colCsv, kvp.Key).ToString();
                    }
                    else
                    {
                        for (int j = 0; j<i; j++)
                        {
                            if (positionCol[j,0] == kvp.Value)
                            {
                                if (positionCol[j,1] != "-1")
                                {
                                    if (lineCsv[int.Parse(positionCol[j, 1])] == "")
                                    {
                                        position = j;
                                    }
                                    else
                                    {
                                        position = -1;
                                    }
                                }
                                else
                                {
                                    position = -1;
                                }
                            }
                        }

                        switch (position)
                        {
                            case 0:
                                positionCol[i, 0] = kvp.Value;
                                positionCol[i, 1] = Array.IndexOf(colCsv, kvp.Key).ToString();
                                break;
                            case -1:
                                positionCol[i, 0] = "-1";
                                positionCol[i, 1] = "-1";
                                position = 0;
                                break;
                            default:
                                positionCol[position, 0] = "-1";
                                positionCol[position, 1] = "-1";
                                positionCol[i, 0] = kvp.Value;
                                positionCol[i, 1] = Array.IndexOf(colCsv, kvp.Key).ToString();
                                position = 0;
                                break;
                        }
                    }

                    i++;
                }

                // Si la clé correspondant à la valeur du code client est absente, on retourne un message
                if (positionCodeCli == -1)
                {
                    retour = "-1";
                }
                else
                {
                    // Si la clé est bien présente, on créé le fichier .imp
                    if (lineCsv[positionCodeCli].Trim() != null)
                    {
                        FicImp = RepImp + "\\IMP" + "\\C_" + lineCsv[positionCodeCli] + ".imp";

                        i = 0;

                        if (File.Exists(FicImp))
                        {
                            File.Delete(FicImp);
                        }

                        using (StreamWriter sw = File.AppendText(FicImp))
                        {
                            for (i = 0; i < dicoD.Count; i++)
                            {
                                if ((int.Parse(positionCol[i, 1]) != -1) && (positionCol[i, 1] != null) && (lineCsv[int.Parse(positionCol[i, 1])] != ""))
                                {
                                    bool monTest = true;
                                    switch (positionCol[i, 0])
                                    {
                                        // Champ obligatoire NOM
                                        case "cli.nom":
                                            if (lineCsv[int.Parse(positionCol[i, 1])].Length == 0)
                                            {
                                                retour = "Erreur : Nom absent pour la fiche client n°" + lineCsv[positionCodeCli];
                                            }
                                            else if (lineCsv[int.Parse(positionCol[i, 1])].Length > 50)
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])].Substring(0, 50);
                                            }
                                            else
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])];
                                            }
                                            break;

                                        // Champs limités à plus de 100 caractères
                                        case "cli.obs":
                                            if (lineCsv[int.Parse(positionCol[i, 1])].Length > 30000)
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])].Substring(0, 30000);
                                            }
                                            else
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])];
                                            }
                                            break;

                                        // Champs limités à 50 caractères
                                        case "cli.titre":
                                        case "cli.prenom":
                                        case "cli.adr1":
                                        case "cli.adr2":
                                        case "cli.ville":
                                        case "cli.pays":
                                        case "cli.mel":
                                        case "cli.origine":
                                        case "cli.zone":
                                        case "cli.siteweb":
                                        case "cli.nombanque":
                                        case "cli.iban":
                                        case "cli.info1":
                                        case "cli.info2":
                                        case "cli.info3":
                                        case "cli.info4":
                                        case "cli.info5":
                                        case "cli.info6":
                                        case "cli.info7":
                                        case "cli.info8":
                                        case "cli.info9":
                                        case "cli.info10":
                                        case "cli.info11":
                                        case "cli.info12":
                                        case "cli.info13":
                                        case "cli.info14":
                                        case "cli.info15":
                                        case "cli.com1":
                                        case "cli.com2":
                                        case "cli.syncid":
                                            if (lineCsv[int.Parse(positionCol[i, 1])].Length > 50)
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])].Substring(0, 50);
                                            }
                                            else
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])];
                                            }
                                            break;

                                        // Champs limités à 35 caractères
                                        case "cli.coderum":
                                            if (lineCsv[int.Parse(positionCol[i, 1])].Length > 35)
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])].Substring(0, 35);
                                            }
                                            else
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])];
                                            }
                                            break;

                                        // Champs limités à 30 caractères
                                        case "cli.bic":
                                            if (lineCsv[int.Parse(positionCol[i, 1])].Length > 30)
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])].Substring(0, 30);
                                            }
                                            else
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])];
                                            }
                                            break;

                                        // Champs limités à 20 caractères
                                        case "cli.tel":
                                        case "cli.fax":
                                        case "cli.gsm":
                                        case "cli.compta":
                                        case "cli.siret":
                                        case "cli.juridique":
                                        case "cli.codetva":
                                            if (lineCsv[int.Parse(positionCol[i, 1])].Length > 20)
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])].Substring(0, 20);
                                            }
                                            else
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])];
                                            }
                                            break;

                                        // Champ limité à 15 caractères
                                        case "cli.code":
                                            if (lineCsv[int.Parse(positionCol[i, 1])].Length == 0)
                                            {
                                                retour = "Erreur : code client absent pour le client " + lineCsv[positionNomCli] + " " + lineCsv[positionPrenomCli];
                                            }
                                            else if (lineCsv[int.Parse(positionCol[i, 1])].Length > 15)
                                            {
                                                retour = "Erreur : code client trop long pour la fiche client n°" + lineCsv[positionCodeCli] + ". 15 Caractères maximum.";
                                            }
                                            else
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])];
                                            }
                                            break;

                                        // Champs limités à 10 caractères
                                        case "cli.cp":
                                        case "cli.ape":
                                        case "cli.regcode":
                                        case "cli.pflcode":
                                            if (lineCsv[int.Parse(positionCol[i, 1])].Length > 10)
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])].Substring(0, 10);
                                            }
                                            else
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])];
                                            }
                                            break;

                                        // Champs du type du fournisseur
                                        case "cli.typefrn":
                                            if (lineCsv[int.Parse(positionCol[i, 1])] != "PREST")
                                            {
                                                retour = "Erreur : Type fournisseur = PREST ou vide pour la fiche fournisseur n°" + lineCsv[positionCodeCli];
                                            }
                                            else
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])];
                                            }
                                            break;

                                        // Champs de date
                                        case "cli.dtcree":
                                            monTest = true;
                                            try
                                            {
                                                DateTime retourDt = System.Convert.ToDateTime(lineCsv[int.Parse(positionCol[i, 1])] + " 00:00:00", culture);
                                            }
                                            catch
                                            {
                                                retour = "Erreur : Format de date de création invalide pour la fiche n°" + lineCsv[positionCodeCli] + ". Format attendu jj/MM/aaaa.";
                                                monTest = false;
                                            }
                                            finally
                                            {
                                                if (monTest)
                                                {
                                                    retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])];
                                                }
                                            }
                                            break;
                                        case "cli.daterum":
                                            monTest = true;
                                            try
                                            {
                                                DateTime retourDt = System.Convert.ToDateTime(lineCsv[int.Parse(positionCol[i, 1])] + " 00:00:00", culture);
                                            }
                                            catch
                                            {
                                                retour = "Erreur : Format de date de la signature du mandat du virement SEPA invalide pour la fiche n°" + lineCsv[positionCodeCli] + ". Format attendu jj/MM/aaaa.";
                                                monTest = false;
                                            }
                                            finally
                                            {
                                                if (monTest)
                                                {
                                                    retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])];
                                                }
                                            }
                                            break;

                                        // Champs numérique AGENCE égal à 4 caractères
                                        case "cli.agence":
                                            if (lineCsv[int.Parse(positionCol[i, 1])].Length < 4)
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])].PadRight(4, '_');
                                            }
                                            else if (lineCsv[int.Parse(positionCol[i, 1])].Length > 4)
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])].Substring(0, 4);
                                            }
                                            else
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])];
                                            }
                                            break;

                                        // Champs numérique SOCIETE égal à 3 caractères
                                        case "cli.societe":
                                            if (lineCsv[int.Parse(positionCol[i, 1])].Length < 3)
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])].PadRight(3, '0');
                                            }
                                            else if (lineCsv[int.Parse(positionCol[i, 1])].Length > 3)
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])].Substring(0, 3);
                                            }
                                            else
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])];
                                            }
                                            break;

                                        // Champs numériques
                                        case "cli.numtaxedefaut":
                                            if ((lineCsv[int.Parse(positionCol[i, 1])] != "1") || (lineCsv[int.Parse(positionCol[i, 1])] != "2") || (lineCsv[int.Parse(positionCol[i, 1])] != "3") || (lineCsv[int.Parse(positionCol[i, 1])] != "4"))
                                            {
                                                retour = "Erreur : le N° de taxe par défaut doit être compris entre 1 et 4 pour la fiche client n°" + lineCsv[positionCodeCli];
                                            }
                                            else
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])];
                                            }
                                            break;
                                        case "cli.codelock":
                                            if ((lineCsv[int.Parse(positionCol[i, 1])] != "0") || (lineCsv[int.Parse(positionCol[i, 1])] != "1"))
                                            {
                                                retour = "Erreur : le code de blocage du code client doit être égal à 0 (non vérouillé) ou 1 (vérouillé) pour la fiche client n°" + lineCsv[positionCodeCli];
                                            }
                                            else
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])];
                                            }
                                            break;
                                        case "cli.typecode":
                                            if ((lineCsv[int.Parse(positionCol[i, 1])] != "1") || (lineCsv[int.Parse(positionCol[i, 1])] != "2") || (lineCsv[int.Parse(positionCol[i, 1])] != "3"))
                                            {
                                                retour = "Erreur : le code du type client doit être égal à 1 (simple adresse) ou 2 (prospect) ou 3 (client) pour la fiche client n°" + lineCsv[positionCodeCli];
                                            }
                                            else
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])];
                                            }
                                            break;
                                        case "cli.etatcode":
                                            if ((lineCsv[int.Parse(positionCol[i, 1])] != "1") || (lineCsv[int.Parse(positionCol[i, 1])] != "2") || (lineCsv[int.Parse(positionCol[i, 1])] != "3"))
                                            {
                                                retour = "Erreur : le code du type client doit être égal à 1 (actif) ou 2 (inactif) ou 3 (n'existe plus) pour la fiche client n°" + lineCsv[positionCodeCli];
                                            }
                                            else
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])];
                                            }
                                            break;

                                        // Champs pour la valeur de remise
                                        case "cli.valrem0":
                                        case "cli.valrem1":
                                        case "cli.valrem2":
                                        case "cli.valrem3":
                                        case "cli.valrem4":
                                        case "cli.valrem5":
                                        case "cli.valrem6":
                                        case "cli.valrem7":
                                        case "cli.valrem8":
                                        case "cli.valrem9":
                                        case "cli.valrem10":
                                        case "cli.valrem11":
                                        case "cli.valrem12":
                                        case "cli.valrem13":
                                        case "cli.valrem14":
                                        case "cli.valrem15":
                                        case "cli.valrem16":
                                        case "cli.valrem17":
                                        case "cli.valrem18":
                                        case "cli.valrem19":
                                        case "cli.valrem20":
                                            if (int.Parse(lineCsv[int.Parse(positionCol[i, 1])]) > 100)
                                            {
                                                retour = "Erreur : l'une des valeurs de remise du client est supérieure à 100 pour la fiche client n°" + lineCsv[positionCodeCli];
                                            }
                                            else
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])];
                                            }
                                            break;

                                        // Champs pour la valeur du coefficient de vente
                                        case "cli.valcoef0":
                                        case "cli.valcoef1":
                                        case "cli.valcoef2":
                                        case "cli.valcoef3":
                                        case "cli.valcoef4":
                                        case "cli.valcoef5":
                                        case "cli.valcoef6":
                                        case "cli.valcoef7":
                                        case "cli.valcoef8":
                                        case "cli.valcoef9":
                                        case "cli.valcoef10":
                                        case "cli.valcoef11":
                                        case "cli.valceof12":
                                        case "cli.valcoef13":
                                        case "cli.valceof14":
                                        case "cli.valceof15":
                                        case "cli.valcoef16":
                                        case "cli.valceof17":
                                        case "cli.valceof18":
                                        case "cli.valceof19":
                                        case "cli.valceof20":
                                            if (int.Parse(lineCsv[int.Parse(positionCol[i, 1])]) > 100)
                                            {
                                                retour = "Erreur : l'une des valeurs de remise du client est supérieure à 100 pour la fiche client n°" + lineCsv[positionCodeCli];
                                            }
                                            else
                                            {
                                                retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])];
                                            }
                                            break;

                                        // Autres (nouveaux champs)
                                        default:
                                            retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])];
                                            break;
                                    }

                                    switch (retour.Substring(0, 9))
                                    {
                                        case "Erreur : ":
                                            break;
                                        default:
                                            sw.WriteLine(retour);
                                            break;
                                    }
                                    //if ((positionCol[i, 0] == "cli.agence") && (lineCsv[int.Parse(positionCol[i, 1])].Length < 4))
                                    //{
                                    //    retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])].PadRight(4, '_');
                                    //}
                                    //else
                                    //{
                                    //    retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])];
                                    //}
                                    //sw.WriteLine(retour);
                                }

                            }
                            retour = "Création du fichier C_" + lineCsv[positionCodeCli] + ".imp réussi";
                        }
                    }
                }
            }
            else
            {
                retour = "Code client absent du dictionnaire, variable cli.code.";
            }

            return retour;
        }
    }
}
