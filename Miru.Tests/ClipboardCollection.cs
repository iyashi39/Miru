// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using Xunit;

namespace Miru.Tests
{
    /// <summary>
    /// Tests that use the system clipboard have to be run one after another,
    /// because the clipboard is shared between all the running tests.
    /// </summary>
    [CollectionDefinition("Clipboard", DisableParallelization = true)]
    public class ClipboardCollection
    {
    }
}
