﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ArmoSystems.ArmoGet.HaspClearClient.HaspClearServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="HaspClearServiceReference.IService")]
    public interface IService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/RestartSLM", ReplyAction="http://tempuri.org/IService/RestartSLMResponse")]
        void RestartSLM(string computerName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/RestartSLM", ReplyAction="http://tempuri.org/IService/RestartSLMResponse")]
        System.Threading.Tasks.Task RestartSLMAsync(string computerName);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceChannel : ArmoSystems.ArmoGet.HaspClearClient.HaspClearServiceReference.IService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceClient : System.ServiceModel.ClientBase<ArmoSystems.ArmoGet.HaspClearClient.HaspClearServiceReference.IService>, ArmoSystems.ArmoGet.HaspClearClient.HaspClearServiceReference.IService {
        
        public ServiceClient() {
        }
        
        public ServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void RestartSLM(string computerName) {
            base.Channel.RestartSLM(computerName);
        }
        
        public System.Threading.Tasks.Task RestartSLMAsync(string computerName) {
            return base.Channel.RestartSLMAsync(computerName);
        }
    }
}
