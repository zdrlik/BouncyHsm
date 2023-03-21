﻿using BouncyHsm.Core.Services.Contracts;
using Microsoft.Extensions.Logging;

namespace BouncyHsm.Core.Services.P11Handlers.SpeedAwaiters;

internal class SmardCardSpeedAwaiter : BaseSpeedAwaiter
{
    public SmardCardSpeedAwaiter(ITimeAccessor timeAccessor, ILogger<SmardCardSpeedAwaiter> logger)
        : base(timeAccessor, logger)
    {
    }

    protected override double[] GetMultiplicationVector()
    {
        return new double[] { 23.2, 36.0, 36.0 };
    }
}