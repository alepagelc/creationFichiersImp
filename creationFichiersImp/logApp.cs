using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace creationFichiersImp
{
    public class logApp
    {
        public void DumpLog(StreamReader r)
        {
            String line;

            line = r.ReadLine();
            while (line != null)
            {
                line = r.ReadLine();
            }

        }

        public void EffacerFichier(String monFichier)
        {
            using (StreamReader r = File.OpenText(monFichier))
            {
                DumpLog(r);
            }
        }

        public void Log(String logMessage, String fic)
        {
            using (StreamWriter w = File.AppendText(fic))
            {
                DateTime maintenant = DateTime.Now;
                
                w.WriteLine(maintenant.ToString("dd-MM-yyyy HH:mm:ss") + " : " + logMessage);
            }
        }
    }
}
