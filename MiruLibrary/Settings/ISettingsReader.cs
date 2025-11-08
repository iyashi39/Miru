// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using System;

namespace MiruLibrary.Settings
{
    public interface ISettingsReader
    {
        T Load<T>() where T : class, new();
        object Load(Type type);
    }
}
