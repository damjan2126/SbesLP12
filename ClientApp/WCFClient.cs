using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Contracts;
using Manager;
using System.Security.Principal;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;

namespace ClientApp
{
	public class WCFClient : ChannelFactory<IWCFContract>, IWCFContract, IDisposable
	{
		IWCFContract factory;

		public WCFClient(NetTcpBinding binding, EndpointAddress address, string crtName)
			: base(binding, address)
		{
			/// cltCertCN.SubjectName should be set to the client's username. .NET WindowsIdentity class provides information about Windows user running the given process
			string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
						
			this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
			this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
			this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

			/// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
			this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, crtName);

			factory = this.CreateChannel();
		}

		public void Dispose()
		{
			if (factory != null)
			{
				factory = null;
			}

			this.Close();
		}

		public void Login()
		{
			try
			{
                factory.Login();
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Error while trying to log in : {0}", e.Detail.Message);
            }
            catch (FaultException<ServerException> e)
            {
                Console.WriteLine("Error while trying to log in : {0}", e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Base exception while logging in : {0}", e.Message);
            }

        }

		public void SendText(byte[] dataArray)
		{
            try
            {
                factory.SendText(dataArray);
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Error while trying to send text : {0}", e.Detail.Message);
            }
            catch (FaultException<ServerException> e)
            {
                Console.WriteLine("Error while trying to send text : {0}", e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Base exception while sending text : {0}", e.Message);
            }
        }

		public void SendMessage(string message)
		{
			byte[] encryptedData = AESMethods.EncryptText(message);

			SendText(encryptedData);
		}

		public void Logout()
		{
            try
            {
                factory.Logout();
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Error while trying to logging out: {0}", e.Detail.Message);
            }
            catch (FaultException<ServerException> e)
            {
                Console.WriteLine("Error while trying to logging out: {0}", e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Base exception while logging out: {0}", e.Message);
            }
            
		}

		public Dictionary<string, List<string>> GetAll(int minCharacters = 0)
		{
            try
            {
                var messages = factory.GetAll(minCharacters);

                foreach (KeyValuePair<string, List<string>> entry in messages)
                {
                    foreach (string msg in entry.Value)
                    {
                        Console.WriteLine($"User {entry.Key} sent message: ");
                        Console.WriteLine($"{msg}");
                    }
                }
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Error while trying to get all messages: {0}", e.Detail.Message);
                
            }
            catch (FaultException<ServerException> e)
            {
                Console.WriteLine("Error while trying to get all messages: {0}", e.Detail.Message);

            }
            catch (Exception e)
            {
                Console.WriteLine("Base exception while trying to get all messages: {0}", e.Message);
            }
            return null;
        }

        public void PunishStudent(string username)
		{
            try
            {
                factory.PunishStudent(username);
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Error while trying to punish user: {0}", e.Detail.Message);
            }
            catch (FaultException<ServerException> e)
            {
                Console.WriteLine("Error while trying to punish user: {0}", e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Base exception while punishing user: {0}", e.Message);
            }
        }

        public void ForgiveStudent(string username)
        {
            try
            {
                factory.ForgiveStudent(username);
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Error while trying to punish user: {0}", e.Detail.Message);
            }
            catch (FaultException<ServerException> e)
            {
                Console.WriteLine("Error while trying to punish user: {0}", e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Base exception while punishing user: {0}", e.Message);
            }
        }
    }
}
