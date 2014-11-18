
namespace Vancl.TMS.Model
{
    /// <summary>
    /// 可写日志接口
    /// </summary>
    public interface ILogable
    {
        /// <summary>
        /// 日志记录的主键值
        /// </summary>
        string PrimaryKey { get; set; }
    }
}
