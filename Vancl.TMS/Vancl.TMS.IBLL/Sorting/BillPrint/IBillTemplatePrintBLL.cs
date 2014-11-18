using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.BillPrint;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IBLL.Sorting.BillPrint
{
    public interface IBillTemplatePrintBLL
    {
        /// <summary>
        /// 获取所有打印元素节点
        /// </summary>
        /// <returns></returns>
        IList<BillPrintFieldModel> GetBillPrintFields();
        /// <summary>
        /// 添加打印元素
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel AddBillPrintField(BillPrintFieldModel model);

        /// <summary>
        /// 根据配送商编码获取打印模板编码
        /// </summary>
        /// <param name="distributionCode"></param>
        /// <returns></returns>
        IList<BillPrintTemplateModel> GetBillPrintTemplates(string distributionCode);

        /// <summary>
        /// 获取打印模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BillPrintTemplateModel GetBillPrintTemplate(long id);

        /// <summary>
        /// 获取打印模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResultModel<PrintTemplateArgModel> SaveBillPrintTemplate(PrintTemplateArgModel model);

        /// <summary>
        /// 删除打印模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResultModel DeletePrintTemplate(long id);

        /// <summary>
        /// 根据模板和数据生成打印内容
        /// </summary>
        /// <param name="tpl"></param>
        /// <param name="data"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        IList<string> RenderPrintData(long tpl, string data, List<int> row);

        /// <summary>
        /// 获取模板内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetBillPrintTemplateData(long id);

        /// <summary>
        /// 填充模板内容
        /// </summary>
        /// <param name="Template"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        string FillTemplateData(string tpl, PrintBillNewModel model);
    }
}
