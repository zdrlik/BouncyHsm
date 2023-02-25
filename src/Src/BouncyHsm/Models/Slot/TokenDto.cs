﻿namespace BouncyHsm.Models.Slot;

public class TokenDto
{
    public string Label
    {
        get;
        set;
    }

    public string SerialNumber
    {
        get;
        set;
    }

    public bool SimulateHwRng
    {
        get;
        set;
    }

    public bool SimulateHwMechanism
    {
        get;
        set;
    }

    public bool SimulateQualifiedArea
    {
        get;
        set;
    }

    public bool IsUserPinLocked
    {
        get;
        set;
    }

    public bool IsSoPinLocked
    {
        get;
        set;
    }

    public TokenDto()
    {
        this.SerialNumber = string.Empty;
        this.Label = string.Empty;
    }
}