﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChessOnlineWebAPI.EmailServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="EmailServiceReference.IEmailService")]
    public interface IEmailService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEmailService/SendEmail", ReplyAction="http://tempuri.org/IEmailService/SendEmailResponse")]
        void SendEmail(string email_id, string subject, string message, bool isMessageHtml);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEmailService/SendEmail", ReplyAction="http://tempuri.org/IEmailService/SendEmailResponse")]
        System.Threading.Tasks.Task SendEmailAsync(string email_id, string subject, string message, bool isMessageHtml);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IEmailServiceChannel : ChessOnlineWebAPI.EmailServiceReference.IEmailService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class EmailServiceClient : System.ServiceModel.ClientBase<ChessOnlineWebAPI.EmailServiceReference.IEmailService>, ChessOnlineWebAPI.EmailServiceReference.IEmailService {
        
        public EmailServiceClient() {
        }
        
        public EmailServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public EmailServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EmailServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EmailServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void SendEmail(string email_id, string subject, string message, bool isMessageHtml) {
            base.Channel.SendEmail(email_id, subject, message, isMessageHtml);
        }
        
        public System.Threading.Tasks.Task SendEmailAsync(string email_id, string subject, string message, bool isMessageHtml) {
            return base.Channel.SendEmailAsync(email_id, subject, message, isMessageHtml);
        }
    }
}
