using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace creationFichiersImp
{
    public class ParseCsv
    {
        private string[] headerCsv;
        private string[] lineCsv;
        private List<string[]> linesCsv = new List<string[]>();

        public bool TraitementCsvToObj (string cheminFic)
        {
            bool succes = false;

            if (File.Exists(cheminFic))
            {
                linesCsv.Clear();

                using (StreamReader fs = new StreamReader(cheminFic, Encoding.GetEncoding("iso-8859-1")))
                {
                    string ligHeader = fs.ReadLine();

                    headerCsv = ligHeader.Split(';');

                    while(fs.Peek() >= 0)
                    {
                        lineCsv = fs.ReadLine().Split(';');
                        linesCsv.Add(lineCsv);
                    }
                }

                succes = true;
            }

            return succes;
        }

        public string[] GetHeader()
        {
            return headerCsv;
        }

        public List<string[]> GetLines()
        {
            return linesCsv;
        }
    }
}
