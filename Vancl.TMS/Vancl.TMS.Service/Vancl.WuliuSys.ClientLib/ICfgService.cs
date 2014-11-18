using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Vancl.WuliuSys.ClientLib
{
    [ServiceContract(Namespace = "http://www.wuliusys.com")]
    public interface ICfgService
    {
        [OperationContract]
        string Version();
        [OperationContract]
        string Test();
    }
}
