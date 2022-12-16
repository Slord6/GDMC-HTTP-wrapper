using System;
using System.Collections.Generic;
using System.Text;

namespace GDMCHttp.Commands
{
    public class Say : Command
    {
        public Say(string message) : base("say", new string[] {message})
        {
        }
    }
}
