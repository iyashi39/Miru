// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MiruLibrary.Services
{
    [ExcludeFromCodeCoverage]
    public class TimerService : ITimerService
    {
        public async Task DelayTask(int millisecondsDelay)
        {
            await Task.Delay(millisecondsDelay);
        }
    }
}
