namespace Vancl.TMS.Schedule.Lms2TmsService
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
            this.Lms2TmsServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.Lms2TmsServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // Lms2TmsServiceProcessInstaller
            // 
            this.Lms2TmsServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.Lms2TmsServiceProcessInstaller.Password = null;
            this.Lms2TmsServiceProcessInstaller.Username = null;
            // 
            // Lms2TmsServiceInstaller
            // 
            this.Lms2TmsServiceInstaller.Description = "Lms2Tms[从主库抓取分拣数据]";
            this.Lms2TmsServiceInstaller.DisplayName = "Vancl.TMS.Schedule.Lms2TmsService";
            this.Lms2TmsServiceInstaller.ServiceName = "Lms2TmsService";
            this.Lms2TmsServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.Lms2TmsServiceProcessInstaller,
            this.Lms2TmsServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller Lms2TmsServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller Lms2TmsServiceInstaller;
    }
}