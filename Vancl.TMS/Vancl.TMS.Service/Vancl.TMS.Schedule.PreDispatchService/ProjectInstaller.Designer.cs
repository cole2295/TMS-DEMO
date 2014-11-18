namespace Vancl.TMS.Schedule.PreDispatchService
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
            this.PreDispatchServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.PreDispatchServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // PreDispatchServiceProcessInstaller
            // 
            this.PreDispatchServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.PreDispatchServiceProcessInstaller.Password = null;
            this.PreDispatchServiceProcessInstaller.Username = null;
            // 
            // PreDispatchServiceInstaller
            // 
            this.PreDispatchServiceInstaller.Description = "TMS预调度服务";
            this.PreDispatchServiceInstaller.DisplayName = "Vancl.TMS.Schedule.PreDispatchService";
            this.PreDispatchServiceInstaller.ServiceName = "PreDispatchService";
            this.PreDispatchServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.PreDispatchServiceProcessInstaller,
            this.PreDispatchServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller PreDispatchServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller PreDispatchServiceInstaller;
    }
}