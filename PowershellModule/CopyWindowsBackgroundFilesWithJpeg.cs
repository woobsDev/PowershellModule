namespace PowershellModule
{ 
    using System;
    using System.IO;
    using System.Management.Automation;

    [Cmdlet(VerbsCommon.Copy, "WindowsBackgroundFilesWithJpeg")]
    public class CopyWindowsBackgroundFilesWithJpeg : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Path to copy files from Localstate\\Assets and rename.")]
        public string DestinationPath { get; set; }

        string createDirectoryChoice;
        string systemDirectoryWithPictures;
        string[] filesToCopy;

        protected void CopyFilesAndRename(string sourceFile, string destination)
        {
            try
            {
                File.Copy(sourceFile, Path.Combine(destination, Path.GetFileName(sourceFile) + ".jpeg"));
            }
            catch (IOException err)
            {
                WriteObject(err);
            }
        }

        protected override void BeginProcessing()
        {
            // Hard coded for testing  //
            //this.DestinationPath = @"C:\temp\wallpaperDesktops\";
            //this.Path = this.SessionState.Path.CurrentLocation.ToString();
            this.systemDirectoryWithPictures = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
                + @"\AppData\Local\Packages\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\LocalState\Assets";

            this.filesToCopy = Directory.GetFiles(systemDirectoryWithPictures);
        }

        protected override void ProcessRecord()
        {
            /* Test path where files reside
            WriteObject(systemDirectoryWithPictures);
            WriteObject(DestinationPath.ToString());
            */
            if(!Directory.Exists(DestinationPath))
            {
                WriteWarning(DestinationPath+" doesn't exist.  Create directory(Y\\N)?");
                createDirectoryChoice = Console.ReadLine();

                if (createDirectoryChoice.ToUpper() == "Y")
                {
                    Directory.CreateDirectory(DestinationPath);
                    WriteObject(DestinationPath + " created.");


                    WriteObject("Copying files from known locations containing desktop wallpapers to " + DestinationPath);
                    foreach (string file in filesToCopy)
                    {
                        CopyFilesAndRename(file, DestinationPath);
                    }
                }
                else if (createDirectoryChoice.ToUpper() == "N" || createDirectoryChoice != "Y")
                {
                    WriteObject("Exiting...");
                    return;
                }
            }
            else
            {
                WriteObject("Copying files from known locations containing desktop wallpapers to " + DestinationPath);
                foreach (string file in filesToCopy)
                {
                    CopyFilesAndRename(file, DestinationPath);
                }
            }

            // Copy single file.  No iteration needed here.
            CopyFilesAndRename((Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Windows\\Themes\\TranscodedWallpaper").ToString(), DestinationPath);

        }

        protected override void StopProcessing()
        {
            base.StopProcessing();
        }
    }
}
