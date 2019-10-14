namespace PowershellModule
{
    using System.Data;
    using System.IO;
    using System.Management.Automation;

    [Cmdlet(VerbsCommon.Get, "FileList")]
    public class GetFileList : PSCmdlet
    {
        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Path to retrieve files in directory.")]
        public string Path { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = false,
            ValueFromPipeline = false,
            Position = 1,
            HelpMessage = "List directories")]
        public SwitchParameter ListDirectories { get; set; }

        private bool listDirectorySwitchValue;

        protected override void BeginProcessing()
        {
            listDirectorySwitchValue = ListDirectories.IsPresent;
        }

        protected override void ProcessRecord()
        {
            if(!Directory.Exists(Path))
            {
                WriteWarning("Directory does not exist!  Exiting...");
                return;
            }
            else
            {
                switch(listDirectorySwitchValue)
                {
                    case true:
                        WriteObject(Directory.GetDirectories(Path));
                        break;

                    case false:
                        WriteObject(Directory.GetFiles(Path));
                        break;
                }
            }
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}
