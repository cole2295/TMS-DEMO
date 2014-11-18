namespace Vancl.TMS.Schedule.InboundWriteService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.InboundWriteServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.InboundWriteServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // InboundWriteServiceProcessInstaller
            // 
            this.InboundWriteServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.InboundWriteServiceProcessInstaller.Password = null;
            this.InboundWriteServiceProcessInstaller.Username = null;
            // 
            // InboundWriteServiceInstaller
            // 
            this.InboundWriteServiceInstaller.Description = "TMS入库同步写入服务";
            this.InboundWriteServiceInstaller.DisplayName = "Vancl.TMS.Schedule.InboundWriteService";
            this.InboundWriteServiceInstaller.ServiceName = "InboundWriteService";
            this.InboundWriteServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.InboundWriteServiceProcessInstaller,
            this.InboundWriteServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller InboundWriteServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller InboundWriteServiceInstaller;
    }
}