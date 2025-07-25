using System;
using System.IO;
using System.Timers;
using AndroidEnv = Android.OS.Environment;
using Xamarin.Forms;
using TechHive.Services;

[assembly: Dependency(typeof(TechHive.Android.Services.DatabaseCopier))]
namespace TechHive.Android.Services
{
    public class DatabaseCopier : IDatabaseCopier
    {
        private Timer timer;

        public void StartPeriodicCopy()
        {
            timer = new Timer(5000); // 5 seconds
            timer.Elapsed += (s, e) => CopyDb();
            timer.Start();
        }

        private void CopyDb()
        {
            try
            {
                string dbFileName = "TechHive.db3";

                string sourcePath = Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData),
                dbFileName);


                string targetPath = Path.Combine(
                    AndroidEnv.GetExternalStoragePublicDirectory(AndroidEnv.DirectoryDownloads).AbsolutePath,
                    "TechHive-Copy.db3"
                );

                File.Copy(sourcePath, targetPath, true);
                System.Diagnostics.Debug.WriteLine("✅ DB copy success: " + targetPath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("❌ DB copy failed: " + ex.Message);
            }
        }
    }
}