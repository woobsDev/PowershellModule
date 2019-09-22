using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Management.Automation;

namespace PowershellModule
{
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
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
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
            switch(listDirectorySwitchValue)
            {
                case true:
                    if(!Directory.Exists(Path))
                    {
                        WriteObject(this.SessionState.Path.CurrentLocation.ToString());
                    }
                    else
                    {
                        WriteObject(Directory.GetDirectories(Path));
                    }
                    break;

                case false:
                    if (!Directory.Exists(Path))
                    {
                        WriteObject(Directory.GetFiles(this.SessionState.Path.CurrentLocation.ToString()));
                    }
                    else
                    {
                        //WriteObject(Directory.GetDirectories(path));
                        WriteObject(Directory.GetFiles(Path));
                    }
                    break;

            }

            
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}
