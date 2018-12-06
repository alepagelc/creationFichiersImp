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
                                    if ((positionCol[i, 0] == "cli.agence") && (lineCsv[int.Parse(positionCol[i, 1])].Length < 4))
                                    {
                                        retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])].PadRight(4, '_');
                                    }
                                    else
                                    {
                                        retour = positionCol[i, 0] + "=" + lineCsv[int.Parse(positionCol[i, 1])];
                                    }
                                    sw.WriteLine(retour);
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
