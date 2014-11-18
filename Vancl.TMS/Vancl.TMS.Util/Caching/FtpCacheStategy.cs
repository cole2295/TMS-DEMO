using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limilabs.FTP.Client;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Vancl.TMS.Util.IO;

namespace Vancl.TMS.Util.Caching
{
    public class FtpCacheStategy : ICacheStrategy
    {
        public TimeSpan DefaultExpires
        {
            get
            {
                return TimeSpan.Zero;
            }
            set
            {
            }
        }

        private string EncodeKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) key = "#temp#";
            var bytes = Encoding.UTF8.GetBytes(key);
            return Convert.ToBase64String(bytes);
        }

        private Uri GetFtpUri()
        {
            string url = string.Format("ftp://{0}/{1}", FtpCacheStategySettings.Default.Host, FtpCacheStategySettings.Default.Folder);
            var uri = new Uri(url);
            return uri;
        }

        public void Set(string key, object value)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (!value.GetType().IsSerializable) throw new ArgumentException("传入的对象必须是可序列化对象");
            var newkey = EncodeKey(key);
            using (FtpHelper ftp = new FtpHelper(FtpAction.UpLoad, newkey, GetFtpUri(), FtpCacheStategySettings.Default.UserName, FtpCacheStategySettings.Default.Password))
            {

                //}
                //using (Ftp ftp = new Ftp())
                //{
                //ftp.Connect(FtpCacheStategySettings.Default.Host);
                //ftp.Login(FtpCacheStategySettings.Default.UserName, FtpCacheStategySettings.Default.Password);
                //ftp.ChangeFolder(FtpCacheStategySettings.Default.Folder);
                MemoryStream rems = new MemoryStream();
                BinaryFormatter binFmt = new BinaryFormatter();
                binFmt.Serialize(rems, value);
                rems.Seek(0, SeekOrigin.Begin);
                ftp.LocalFileStream = rems;
                ftp.Action();
                //ftp.Upload(newkey, rems);
                //ftp.Close();
            }

        }

        public void Set(string key, object value, TimeSpan timeSpan)
        {
            this.Set(key, value);
        }

        public void Set(string key, object value, DateTime dateTime)
        {
            this.Set(key, value);
        }

        public object Get(string key)
        {
            var newkey = EncodeKey(key);
            using (FtpHelper ftp = new FtpHelper(FtpAction.DownLoad, newkey, GetFtpUri(), FtpCacheStategySettings.Default.UserName, FtpCacheStategySettings.Default.Password))
            {
                ftp.Action();
                BinaryFormatter binFmt = new BinaryFormatter();
                Stream rems = ftp.FileStream;
                rems.Seek(0, SeekOrigin.Begin);
                return binFmt.Deserialize(rems);
            }
        }

        public T Get<T>(string key)
        {
            var data = Get(key);
            if (data == null) return default(T);
            return (T)data;
        }

        public object Remove(string key)
        {
            var newkey = EncodeKey(key);
            using (FtpHelper ftp = new FtpHelper(FtpAction.Delete, newkey, GetFtpUri(), FtpCacheStategySettings.Default.UserName, FtpCacheStategySettings.Default.Password))
            {
                ftp.Action();
                return null;
            }
        }

        public T Remove<T>(string key)
        {
            return (T)Remove(key);
        }
    }
}
