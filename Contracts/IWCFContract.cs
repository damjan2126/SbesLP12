using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Contracts
{
	[ServiceContract]
	public interface IWCFContract
	{
		[OperationContract]
        [FaultContract(typeof(SecurityException))]
        [FaultContract(typeof(ServerException))]
        void Login();

		[OperationContract]
        [FaultContract(typeof(SecurityException))]
        [FaultContract(typeof(ServerException))]
        void SendText(byte[] dataArray);

		[OperationContract]
        [FaultContract(typeof(SecurityException))]
        [FaultContract(typeof(ServerException))]
        void Logout();

		[OperationContract]
        [FaultContract(typeof(SecurityException))]
        [FaultContract(typeof(ServerException))]
        Dictionary<string, List<string>> GetAll(int minCharacters = 0);

		[OperationContract]
        [FaultContract(typeof(SecurityException))]
        [FaultContract(typeof(ServerException))]
        void PunishStudent(string username);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        [FaultContract(typeof(ServerException))]
        void ForgiveStudent(string username);

    }
}
