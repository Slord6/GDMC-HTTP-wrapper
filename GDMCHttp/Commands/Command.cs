using System;
using System.Collections.Generic;
using System.Text;

namespace GDMCHttp.Commands
{
    public class Command : ICommand
    {
        private string command;
        private string[] args;

        public Command(string command, string[] args)
        {
            this.command = command;
            this.args = args;
        }

        public string ToCommandString()
        {
            return command + " " + string.Join(" ", args);
        }
    }
}
