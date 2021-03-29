using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AuthenticationService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAuthenticationService" in both code and config file together.
    [ServiceContract]
    public interface IAuthenticationService
    {
        [OperationContract]
        [FaultContract(typeof(AuthenticationFault))]
        string AreCorrectCredentials(string username, string password);
    }

	[DataContract]
    public class AuthenticationFault 
    {
        [Flags]
        public enum AuthenticationFaultType
		{
            [EnumMember]
            NoSuchUser,
            [EnumMember]
            InvalidPassword,
            [EnumMember]
            ServerFault
		}

		[DataMember]
        public AuthenticationFaultType FaultType { get; protected set; }

        public AuthenticationFault(AuthenticationFaultType type)
        {
            FaultType = type;
        }
    }
}
