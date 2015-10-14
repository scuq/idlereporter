using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace idlereporter
{
    class Presence
    {
        public static void setIdle(string toFileDirectory, bool idle)
        {
            

            if (idle == true)
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(toFileDirectory + "\\" + System.Environment.MachineName);
                file.WriteLine("notidle");
                file.Close();
            } else
            {
                System.IO.File.Delete(toFileDirectory + "\\" + System.Environment.MachineName);
            }


            
        }
    }
}
