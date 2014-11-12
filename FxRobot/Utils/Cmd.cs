using System;
using System.Collections.Generic;
using System.Text;

namespace Haozes.Robot.Utils
{
   public class Cmd
    {
       protected string name = string.Empty;
       public string Name
       {
           get { return name; }
           set { name = value; }
       }

       protected string para = string.Empty;
       public string Para
       {
           get { return para; }
           set { para = value; }
       }
    }

   public class InterCmd : Cmd
   {
       private InternaCmdName interCmdName;
       public InternaCmdName InterName
       {
           get { return this.interCmdName; }
           set { this.interCmdName = value; }
       }

       public InterCmd()
       { 
       }

       public InterCmd(string name, string para)
       {
           this.interCmdName = (InternaCmdName)Enum.Parse(typeof(InternaCmdName), name, true);
           this.para = para;
       }
   }
}
