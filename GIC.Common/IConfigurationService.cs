﻿using System.Collections.Generic;

namespace GIC.Common.Services
{
    public interface IConfigurationService
    {
        List<string> Applications { get; set; }
        string Password { get; set; }
        ushort Port { get; set; }
        int SelectedApp { get; set; }
    }
}