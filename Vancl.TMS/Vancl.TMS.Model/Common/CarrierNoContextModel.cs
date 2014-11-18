using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Common
{
    public class CarrierNoContextModel
    {
        public SerialNumberModel NumberModel
        {
            get
            {
                SerialNumberModel m = new SerialNumberModel();
                m.FillerCharacter = "0";
                m.NumberLength = 4;
                return m;
            }
        }

        private Enums.CarrierCoverage _carrierCoverage;
        public Enums.CarrierCoverage CarrierCoverage
        {
            get
            {
                return _carrierCoverage;
            }
        }

        private string _carrierNo;
        public string CarrierNo
        {
            get
            {
                return _carrierNo;
            }
        }

        public CarrierNoContextModel(Enums.CarrierCoverage carrierCoverage, string carrierNo)
        {
            _carrierCoverage = carrierCoverage;
            _carrierNo = carrierNo;
        }


    }
}
