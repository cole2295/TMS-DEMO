﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.269
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vancl.TMS.BLL.SMSService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.vancl.com/", ConfigurationName="SMSService.CommonMsgSoap")]
    public interface CommonMsgSoap {
        
        // CODEGEN: 命名空间 http://www.vancl.com/ 的元素名称 mobile 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://www.vancl.com/AddMsgByTitle", ReplyAction="*")]
        Vancl.TMS.BLL.SMSService.AddMsgByTitleResponse AddMsgByTitle(Vancl.TMS.BLL.SMSService.AddMsgByTitleRequest request);
        
        // CODEGEN: 命名空间 http://www.vancl.com/ 的元素名称 mobile 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://www.vancl.com/AddMsgBySign", ReplyAction="*")]
        Vancl.TMS.BLL.SMSService.AddMsgBySignResponse AddMsgBySign(Vancl.TMS.BLL.SMSService.AddMsgBySignRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class AddMsgByTitleRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="AddMsgByTitle", Namespace="http://www.vancl.com/", Order=0)]
        public Vancl.TMS.BLL.SMSService.AddMsgByTitleRequestBody Body;
        
        public AddMsgByTitleRequest() {
        }
        
        public AddMsgByTitleRequest(Vancl.TMS.BLL.SMSService.AddMsgByTitleRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.vancl.com/")]
    public partial class AddMsgByTitleRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string mobile;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string content;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string title;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string department;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string key;
        
        public AddMsgByTitleRequestBody() {
        }
        
        public AddMsgByTitleRequestBody(string mobile, string content, string title, string department, string key) {
            this.mobile = mobile;
            this.content = content;
            this.title = title;
            this.department = department;
            this.key = key;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class AddMsgByTitleResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="AddMsgByTitleResponse", Namespace="http://www.vancl.com/", Order=0)]
        public Vancl.TMS.BLL.SMSService.AddMsgByTitleResponseBody Body;
        
        public AddMsgByTitleResponse() {
        }
        
        public AddMsgByTitleResponse(Vancl.TMS.BLL.SMSService.AddMsgByTitleResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.vancl.com/")]
    public partial class AddMsgByTitleResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int AddMsgByTitleResult;
        
        public AddMsgByTitleResponseBody() {
        }
        
        public AddMsgByTitleResponseBody(int AddMsgByTitleResult) {
            this.AddMsgByTitleResult = AddMsgByTitleResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class AddMsgBySignRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="AddMsgBySign", Namespace="http://www.vancl.com/", Order=0)]
        public Vancl.TMS.BLL.SMSService.AddMsgBySignRequestBody Body;
        
        public AddMsgBySignRequest() {
        }
        
        public AddMsgBySignRequest(Vancl.TMS.BLL.SMSService.AddMsgBySignRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.vancl.com/")]
    public partial class AddMsgBySignRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string mobile;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string content;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string title;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string department;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string msgsign;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string key;
        
        public AddMsgBySignRequestBody() {
        }
        
        public AddMsgBySignRequestBody(string mobile, string content, string title, string department, string msgsign, string key) {
            this.mobile = mobile;
            this.content = content;
            this.title = title;
            this.department = department;
            this.msgsign = msgsign;
            this.key = key;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class AddMsgBySignResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="AddMsgBySignResponse", Namespace="http://www.vancl.com/", Order=0)]
        public Vancl.TMS.BLL.SMSService.AddMsgBySignResponseBody Body;
        
        public AddMsgBySignResponse() {
        }
        
        public AddMsgBySignResponse(Vancl.TMS.BLL.SMSService.AddMsgBySignResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.vancl.com/")]
    public partial class AddMsgBySignResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int AddMsgBySignResult;
        
        public AddMsgBySignResponseBody() {
        }
        
        public AddMsgBySignResponseBody(int AddMsgBySignResult) {
            this.AddMsgBySignResult = AddMsgBySignResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface CommonMsgSoapChannel : Vancl.TMS.BLL.SMSService.CommonMsgSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CommonMsgSoapClient : System.ServiceModel.ClientBase<Vancl.TMS.BLL.SMSService.CommonMsgSoap>, Vancl.TMS.BLL.SMSService.CommonMsgSoap {
        
        public CommonMsgSoapClient() {
        }
        
        public CommonMsgSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CommonMsgSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CommonMsgSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CommonMsgSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Vancl.TMS.BLL.SMSService.AddMsgByTitleResponse Vancl.TMS.BLL.SMSService.CommonMsgSoap.AddMsgByTitle(Vancl.TMS.BLL.SMSService.AddMsgByTitleRequest request) {
            return base.Channel.AddMsgByTitle(request);
        }
        
        public int AddMsgByTitle(string mobile, string content, string title, string department, string key) {
            Vancl.TMS.BLL.SMSService.AddMsgByTitleRequest inValue = new Vancl.TMS.BLL.SMSService.AddMsgByTitleRequest();
            inValue.Body = new Vancl.TMS.BLL.SMSService.AddMsgByTitleRequestBody();
            inValue.Body.mobile = mobile;
            inValue.Body.content = content;
            inValue.Body.title = title;
            inValue.Body.department = department;
            inValue.Body.key = key;
            Vancl.TMS.BLL.SMSService.AddMsgByTitleResponse retVal = ((Vancl.TMS.BLL.SMSService.CommonMsgSoap)(this)).AddMsgByTitle(inValue);
            return retVal.Body.AddMsgByTitleResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Vancl.TMS.BLL.SMSService.AddMsgBySignResponse Vancl.TMS.BLL.SMSService.CommonMsgSoap.AddMsgBySign(Vancl.TMS.BLL.SMSService.AddMsgBySignRequest request) {
            return base.Channel.AddMsgBySign(request);
        }
        
        public int AddMsgBySign(string mobile, string content, string title, string department, string msgsign, string key) {
            Vancl.TMS.BLL.SMSService.AddMsgBySignRequest inValue = new Vancl.TMS.BLL.SMSService.AddMsgBySignRequest();
            inValue.Body = new Vancl.TMS.BLL.SMSService.AddMsgBySignRequestBody();
            inValue.Body.mobile = mobile;
            inValue.Body.content = content;
            inValue.Body.title = title;
            inValue.Body.department = department;
            inValue.Body.msgsign = msgsign;
            inValue.Body.key = key;
            Vancl.TMS.BLL.SMSService.AddMsgBySignResponse retVal = ((Vancl.TMS.BLL.SMSService.CommonMsgSoap)(this)).AddMsgBySign(inValue);
            return retVal.Body.AddMsgBySignResult;
        }
    }
}
