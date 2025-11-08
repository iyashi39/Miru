// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace MiruLibrary.Services
{
    public class SystemService : ISystemService
    {
        public RegistryKey OpenLocalMachineSubKey(string name)
        {
            return Registry.LocalMachine.OpenSubKey(name);
        }

        [ExcludeFromCodeCoverage]
        public Process StartProcess(string fileName)
        {
            return Process.Start(fileName);
        }

        [ExcludeFromCodeCoverage]
        public void ExitEnvironment(int exitCode)
        {
            Environment.Exit(exitCode);
        }
    }
}
