using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Security.Cryptography.X509Certificates;
using Manager;
using System.Security.Principal;
using System.Threading;
using System.Web;

namespace ClientApp
{
	public class Program
	{
        static void Main(string[] args)
        {
            string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            /// Define the expected service certificate. It is required to establish cmmunication using certificates.

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            binding.MaxReceivedMessageSize = 1000000;
            binding.OpenTimeout = TimeSpan.FromMinutes(2);
            binding.SendTimeout = TimeSpan.FromMinutes(2);
            binding.ReceiveTimeout = TimeSpan.FromMinutes(10);

            /// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, "wcfservice");
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:3241/Receiver"),
                                      new X509CertificateEndpointIdentity(srvCert));

           

            using (WCFClient proxy = new WCFClient(binding, address, "asistent"))
            {
                proxy.Login();
                bool run = true;
                do
                {
                    Console.WriteLine("--------------------------------");
                    Console.WriteLine("Select option: ");
                    Console.WriteLine("1. Send message");
                    Console.WriteLine("2. Get messages with min characters");
                    Console.WriteLine("3. Get all messages");
                    Console.WriteLine("4. Punish student");
                    Console.WriteLine("5. Forgive student");
                    Console.WriteLine("6. Exit");
                    Console.WriteLine("--------------------------------");

                    int input = int.Parse(Console.ReadLine());
                    Console.WriteLine("---------------------------------");
                    switch (input)
                    {
                        case 1:
                            Console.WriteLine("Enter text to send: ");
                            var message = Console.ReadLine();
                            proxy.SendMessage(message);
                            break;
                        case 2:
                            Console.WriteLine("Enter minimum characters");
                            int minChars = int.Parse(Console.ReadLine());
                            Console.WriteLine("-----------------------------------");
                            proxy.GetAll(minChars);
                            Console.WriteLine("-----------------------------------");
                            break;
                        case 3:
                            Console.WriteLine("-----------------------------------");
                            proxy.GetAll();
                            Console.WriteLine("-----------------------------------");
                            break;
                        case 4:
                            Console.WriteLine("Enter username to punish:");
                            var userToPunish = Console.ReadLine();
                            proxy.PunishStudent(userToPunish);
                            break;
                        case 5:
                            Console.WriteLine("Enter username to forgive:");
                            var userToForgive = Console.ReadLine();
                            proxy.ForgiveStudent(userToForgive);
                            break;
                        case 6:
                            Console.WriteLine("Exiting...");
                            proxy.Logout();
                            run = false;
                            break;
                    }
                } while (run);
                proxy.Logout();
            }

        }
    }
    

}
