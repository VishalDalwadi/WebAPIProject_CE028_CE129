﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChessOnlineWebApp.AuthenticationServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="AuthenticationServiceReference.IAuthenticationService")]
    public interface IAuthenticationService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAuthenticationService/AreCorrectCredentials", ReplyAction="http://tempuri.org/IAuthenticationService/AreCorrectCredentialsResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(ChessOnlineWebAPI.AuthenticationServiceReference.AuthenticationFault), Action="http://tempuri.org/IAuthenticationService/AreCorrectCredentialsAuthenticationFaul" +
            "tFault", Name="AuthenticationFault", Namespace="http://schemas.datacontract.org/2004/07/AuthenticationService")]
        string AreCorrectCredentials(string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAuthenticationService/AreCorrectCredentials", ReplyAction="http://tempuri.org/IAuthenticationService/AreCorrectCredentialsResponse")]
        System.Threading.Tasks.Task<string> AreCorrectCredentialsAsync(string username, string password);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IAuthenticationServiceChannel : ChessOnlineWebApp.AuthenticationServiceReference.IAuthenticationService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AuthenticationServiceClient : System.ServiceModel.ClientBase<ChessOnlineWebApp.AuthenticationServiceReference.IAuthenticationService>, ChessOnlineWebApp.AuthenticationServiceReference.IAuthenticationService {
        
        public AuthenticationServiceClient() {
        }
        
        public AuthenticationServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public AuthenticationServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AuthenticationServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AuthenticationServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string AreCorrectCredentials(string username, string password) {
            return base.Channel.AreCorrectCredentials(username, password);
        }
        
        public System.Threading.Tasks.Task<string> AreCorrectCredentialsAsync(string username, string password) {
            return base.Channel.AreCorrectCredentialsAsync(username, password);
        }
    }
}