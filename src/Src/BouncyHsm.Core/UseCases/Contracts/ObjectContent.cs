﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncyHsm.Core.UseCases.Contracts;

public record ObjectContent(string FileName, byte[] Content);
