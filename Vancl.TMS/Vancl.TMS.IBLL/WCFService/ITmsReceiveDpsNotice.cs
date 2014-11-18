using System.Collections.Generic;
using System.ServiceModel;
using Vancl.TMS.Model.Dps;

namespace Vancl.TMS.IBLL.WCFService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ITmsReceiveDpsNotice”。
    [ServiceContract]
    public interface ITmsReceiveDpsNotice
    {
        [OperationContract(IsOneWay = true)]
        void DoNotify(LifeCycleModel model);

        [OperationContract(IsOneWay = true)]
        void DoNotifys(List<LifeCycleModel> models);
    }
}
