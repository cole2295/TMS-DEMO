namespace Vancl.TMS.Schedule.Tms2LmsService
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
            this.Tms2LmsServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.Tms2LmsServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // Tms2LmsServiceProcessInstaller
            // 
            this.Tms2LmsServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.Tms2LmsServiceProcessInstaller.Password = null;
            this.Tms2LmsServiceProcessInstaller.Username = null;
            // 
            // Tms2LmsServiceInstaller
            // 
            this.Tms2LmsServiceInstaller.Description = "Tms2Lms[分拣数据同步回物流主库]";
            this.Tms2LmsServiceInstaller.DisplayName = "Vancl.TMS.Schedule.Tms2LmsService";
            this.Tms2LmsServiceInstaller.ServiceName = "Tms2LmsService";
            this.Tms2LmsServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.Tms2LmsServiceProcessInstaller,
            this.Tms2LmsServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller Tms2LmsServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller Tms2LmsServiceInstaller;
    }
}