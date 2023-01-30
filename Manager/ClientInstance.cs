using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class ClientInstance
    {
        private static ClientInstance instance = null;

        public int Role { get; private set; }
        public string Name { get; private set; }

        public static ClientInstance GetInstance(X509Certificate2 certifitcate = null)
        {
            if (instance == null)
            {
                instance = new ClientInstance(certifitcate);
            }
            return instance;
        }

        public ClientInstance()
        {

        }

        public ClientInstance(X509Certificate2 certifitcate)
        {
            if (certifitcate.SubjectName.Name.Contains("asistentRole"))
            {
                Role = 0;
            }
            else if (certifitcate.SubjectName.Name.Contains("studentRole"))
            {
                Role = 1;
            }
            else
            {
                Role = -1;
            }
            Name = certifitcate.SubjectName.Name.Split(',')[0].Split('=')[1];
        }

        public static void WipeInstance()
        {
            instance = null;
        }


    }
}