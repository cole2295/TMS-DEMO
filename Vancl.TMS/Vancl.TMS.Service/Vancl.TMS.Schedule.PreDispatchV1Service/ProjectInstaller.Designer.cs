namespace Vancl.TMS.Schedule.PreDispatchV1Service
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
            this.PreDispatchV1ServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.PreDispatchV1ServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // PreDispatchV1ServiceProcessInstaller
            // 
            this.PreDispatchV1ServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.PreDispatchV1ServiceProcessInstaller.Password = null;
            this.PreDispatchV1ServiceProcessInstaller.Username = null;
            // 
            // PreDispatchV1ServiceInstaller
            // 
            this.PreDispatchV1ServiceInstaller.Description = "TMS预调度V1服务";
            this.PreDispatchV1ServiceInstaller.DisplayName = "Vancl.TMS.Schedule.PreDispatchV1Service";
            this.PreDispatchV1ServiceInstaller.ServiceName = "PreDispatchV1Service";
            this.PreDispatchV1ServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.PreDispatchV1ServiceProcessInstaller,
            this.PreDispatchV1ServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller PreDispatchV1ServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller PreDispatchV1ServiceInstaller;
    }
}