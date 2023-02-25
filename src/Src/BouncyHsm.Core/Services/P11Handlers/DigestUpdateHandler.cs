﻿using BouncyHsm.Core.Rpc;
using BouncyHsm.Core.Services.Contracts;
using BouncyHsm.Core.Services.Contracts.P11;
using BouncyHsm.Core.Services.P11Handlers.Common;
using BouncyHsm.Core.Services.P11Handlers.States;
using Microsoft.Extensions.Logging;

namespace BouncyHsm.Core.Services.P11Handlers;

public partial class DigestUpdateHandler : IRpcRequestHandler<DigestUpdateRequest, DigestUpdateEnvelope>
{
    private readonly IP11HwServices hwServices;
    private readonly ILogger<DigestUpdateHandler> logger;

    public DigestUpdateHandler(IP11HwServices hwServices, ILogger<DigestUpdateHandler> logger)
    {
        this.hwServices = hwServices;
        this.logger = logger;
    }

    public ValueTask<DigestUpdateEnvelope> Handle(DigestUpdateRequest request, CancellationToken cancellationToken)
    {
        this.logger.LogTrace("Entering to Handle with sessionId {SessionId}.",
            request.SessionId);

        IP11Session p11Session = this.hwServices.ClientAppCtx.EnsureSession(request.AppId, request.SessionId);

        DigestSessionState digestSessionState = p11Session.State.Ensure<DigestSessionState>();
        this.logger.LogDebug("Update digest using {sessionState}.", digestSessionState);

        digestSessionState.Update(request.Data);
        this.logger.LogDebug("Update digest with data length: {dataLength}.", request.Data.Length);

        return new ValueTask<DigestUpdateEnvelope>(new DigestUpdateEnvelope()
        {
            Rv = (uint)CKR.CKR_OK
        });
    }
}
