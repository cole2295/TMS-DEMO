﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18034
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebTestApp.CachingService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Cache", Namespace="http://schemas.datacontract.org/2004/07/Vancl.TMS.Util.CacheService")]
    [System.SerializableAttribute()]
    public partial class Cache : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string CacheNamek__BackingFieldField;
        
        private System.Nullable<System.DateTime> CachePeriodk__BackingFieldField;
        
        private object CacheValuek__BackingFieldField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<CacheName>k__BackingField", IsRequired=true)]
        public string CacheNamek__BackingField {
            get {
                return this.CacheNamek__BackingFieldField;
            }
            set {
                if ((object.ReferenceEquals(this.CacheNamek__BackingFieldField, value) != true)) {
                    this.CacheNamek__BackingFieldField = value;
                    this.RaisePropertyChanged("CacheNamek__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<CachePeriod>k__BackingField", IsRequired=true)]
        public System.Nullable<System.DateTime> CachePeriodk__BackingField {
            get {
                return this.CachePeriodk__BackingFieldField;
            }
            set {
                if ((this.CachePeriodk__BackingFieldField.Equals(value) != true)) {
                    this.CachePeriodk__BackingFieldField = value;
                    this.RaisePropertyChanged("CachePeriodk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<CacheValue>k__BackingField", IsRequired=true)]
        public object CacheValuek__BackingField {
            get {
                return this.CacheValuek__BackingFieldField;
            }
            set {
                if ((object.ReferenceEquals(this.CacheValuek__BackingFieldField, value) != true)) {
                    this.CacheValuek__BackingFieldField = value;
                    this.RaisePropertyChanged("CacheValuek__BackingField");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="CachingService.ICaching")]
    public interface ICaching {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICaching/Store", ReplyAction="http://tempuri.org/ICaching/StoreResponse")]
        void Store(WebTestApp.CachingService.Cache cache);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICaching/Get", ReplyAction="http://tempuri.org/ICaching/GetResponse")]
        WebTestApp.CachingService.Cache Get(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICaching/Remove", ReplyAction="http://tempuri.org/ICaching/RemoveResponse")]
        bool Remove(string name);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICachingChannel : WebTestApp.CachingService.ICaching, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CachingClient : System.ServiceModel.ClientBase<WebTestApp.CachingService.ICaching>, WebTestApp.CachingService.ICaching {
        
        public CachingClient() {
        }
        
        public CachingClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CachingClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CachingClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CachingClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void Store(WebTestApp.CachingService.Cache cache) {
            base.Channel.Store(cache);
        }
        
        public WebTestApp.CachingService.Cache Get(string name) {
            return base.Channel.Get(name);
        }
        
        public bool Remove(string name) {
            return base.Channel.Remove(name);
        }
    }
}