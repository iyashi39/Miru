// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using Microsoft.Win32;
using System.Diagnostics;

namespace MiruLibrary.Services
{
    public interface ISystemService
    {
        void ExitEnvironment(int exitCode);
        RegistryKey OpenLocalMachineSubKey(string name);
        Process StartProcess(string fileName);
    }
}