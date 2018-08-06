using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DatabaseLibrary
{
    public class TextFileClass
    {
        public string WorkPath { get; set; }
        public string FloderName { get; set; }

        public bool CheckFileExists(string FileNmae)
        {
            if (FloderName == "")
            {
                if (File.Exists(WorkPath + "\\" + FileNmae))
                    return true;
                else
                    return false;
            }
            else
            {
                if (File.Exists(WorkPath + "\\" + FloderName + "\\" + FileNmae))
                    return true;
                else
                    return false;
            }
        }

    }
}
