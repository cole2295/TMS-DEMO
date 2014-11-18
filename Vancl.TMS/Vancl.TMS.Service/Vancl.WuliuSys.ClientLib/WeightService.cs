using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Web;
using System.Timers;
using Vancl.WuliuSys.ClientLib.WeighComp;
using System.Threading;

namespace Vancl.WuliuSys.ClientLib
{
    [JavascriptCallbackBehavior(UrlParameterName = "jsoncallback")]
    public class WeightService : IWeightService
    {
        private static decimal weight = 0m;
        private static WeighComponent weighConponent = new WeighComponent();
        static WeightService()
        {
            weighConponent.onWeighChange += new WeighComponent.WeighHandler(weighConponent_onWeighChange);

            //启动称重量组件
            //   weighConponent.Start();
        }

        static void weighConponent_onWeighChange(object sender, WeighEventArgs e)
        {
            weight = e.Weight;
        }

        public WeightService()
        {
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public WeighResult GetWeight()
        {
            if (weighConponent.Status == WeighStatus.Stopped)
            {
                return StartWeigh();
            }
            return new WeighResult
            {
                Weight = weighConponent.CurrentWeight,
                Exception = weighConponent.LastError,
            };
        }


        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public WeighResult StartWeigh()
        {
            WeighResult wr = new WeighResult();
            if (weighConponent.Status == WeighStatus.Running)
            {
                wr.Weight = weighConponent.CurrentWeight;
                wr.Exception = new Exception("称重插件已在运行中");
            }
            else
            {
               var flag = weighConponent.Start();
               if (!flag)
                {
                    wr.Exception = weighConponent.LastError;
                }
                else
                {
                    Thread.Sleep(500);
                    wr.Weight = weighConponent.CurrentWeight;
                    wr.Exception = weighConponent.LastError;
                }
            }
            return wr;
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public bool StopWeigh()
        {
            return weighConponent.Stop();

        }

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public bool CanWeigh()
        {
            return true;
        }
    }
}
