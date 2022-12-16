using System;
using System.Collections.Generic;
using System.Text;

namespace GDMCHttp.Commands
{
    public interface ICommand
    {
        string ToCommandString();
    }
}
