using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using CSharp___DllImport;
using System.IO;
using System.IO.IsolatedStorage;
namespace haret7
{
    public partial class MainPage : PhoneApplicationPage
    {
        public static string HaRETInstallPath = @"\Applications\Data\e96f8b31-3e21-4736-bca7-17fd01203c50\Data\IsolatedStore\haret";
        public static string HaRETBundlePath = @"\Applications\Install\e96f8b31-3e21-4736-bca7-17fd01203c50\Install\haret";
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            checkButtons();
        }
        private void checkButtons()
        {
            if (isHaRETInstalled())
            {
                launchButton.IsEnabled = true;
            }
        }
        public bool isHaRETInstalled()
        {
            if (WP7RootToolsSDK.FileSystem.FileExists(HaRETInstallPath + @"\haret.exe"))
            {
                return true;
            }

            return false;
        }
        public void LaunchHaRET()
        {
            CSharp___DllImport.DllImportCaller.lib.ShellExecuteEx7(HaRETInstallPath + @"\haret.exe", "");
            MessageBox.Show(((CSharp___DllImport.Win32ErrorCode)CSharp___DllImport.DllImportCaller.lib.GetLastError7()).ToString());
            //2048kb
            //1270kb
            //35kb
            //uint haretid = 0;


            //MessageBox.Show(string.Join("\n", Phone.TaskManager.AllProcesses().Select(p => p.RAW.szExeFile + " - " + p.RAW.th32ProcessID).ToArray()));
        }
        public bool IsHaRETRunning()
        {
            var procs = Phone.TaskManager.AllProcesses();
            foreach (var item in procs)
            {
                if (item.RAW.szExeFile == "haret.exe")
                {
                    return true;
                }
            }
            return false;
        }
        public void KillHaRET()
        {
            //var winds = Phone.TaskManager.GetEnumWindows(false, false);
            var procs = Phone.TaskManager.AllProcesses();
            foreach (var item in procs)
            {
                if (item.RAW.szExeFile == "haret.exe")
                {
                    item.Kill(0);
                }
                //foreach (var item2 in winds)
                // {
                //if(item.MemoryInfo ==item2)
                //{

                //}
                // }
            }

        }

        private void launchButton_Click(object sender, RoutedEventArgs e)
        {
            KillHaRET();
            if (isHaRETInstalled())
            {
                LaunchHaRET();
            }
        }

        private void installLatestButton_Click(object sender, RoutedEventArgs e)
        {
            KillHaRET();
            var request = (HttpWebRequest)WebRequest.Create("http://minecraft.digiex.org/jenkins/HaRET-WP7/latest/haret.exe");
            request.UserAgent = "HaRET7 Launcher";
            request.BeginGetResponse(r =>
            {

                var httpRequest = (HttpWebRequest)r.AsyncState;
                var httpResponse = (HttpWebResponse)httpRequest.EndGetResponse(r);
                    using (var myStore = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        if (myStore.FileExists("haret\\haret.exe")) myStore.DeleteFile("haret\\haret.exe");
                        var buffer = new byte[1024];
                        using (var isoStorStr = myStore.OpenFile("haret\\haret.exe", FileMode.CreateNew))
                        {
                            int bytesRead = 0;
                            while ((bytesRead = httpResponse.GetResponseStream().Read(buffer, 0, 1024)) > 0)
                                isoStorStr.Write(buffer, 0, bytesRead);
                        }
                    }

            }, request);
            MessageBox.Show("Download complete.");
            checkButtons();
        }

        private void installLocalButton_Click(object sender, RoutedEventArgs e)
        {
            KillHaRET();
            WP7RootToolsSDK.FileSystem.CreateFolder(HaRETInstallPath);
            WP7RootToolsSDK.Folder folder = WP7RootToolsSDK.FileSystem.GetFolder(HaRETBundlePath);
            List<WP7RootToolsSDK.FileSystemEntry> files = folder.GetSubItems();
            string log = "Installing bundled version...\n";
            foreach (WP7RootToolsSDK.FileSystemEntry file in files)
            {
                if (file.IsFile)
                {
                    log += "Copying file " + file.Name + "...";
                    try
                    {
                        WP7RootToolsSDK.FileSystem.CopyFile(file.Path, HaRETInstallPath + "\\" + file.Name);
                        log += "DONE";
                    }
                    catch (Exception ex)
                    {
                        log += "FAIL! " + ex.Message;
                    }
                    log += "\n";
                }
                else
                {
                    log += "Ignoring folder " + file.Name + "!\n";
                }
            }
            MessageBox.Show(log);
            checkButtons();
        }

        private void viewLogButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not Implemented.");
        }
    }
}