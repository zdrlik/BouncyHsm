﻿using BouncyHsm.Core.Rpc;
using BouncyHsm.Core.Services.Contracts;
using BouncyHsm.Core.Services.Contracts.P11;
using BouncyHsm.Core.Services.P11Handlers.Common;
using BouncyHsm.Core.Services.P11Handlers.States;
using Microsoft.Extensions.Logging;

namespace BouncyHsm.Core.Services.P11Handlers;

public partial class SignFinalHandler : IRpcRequestHandler<SignFinalRequest, SignFinalEnvelope>
{
    private readonly IP11HwServices hwServices;
    private readonly ILogger<SignHandler> logger;

    public SignFinalHandler(IP11HwServices hwServices, ILogger<SignHandler> logger)
    {
        this.hwServices = hwServices;
        this.logger = logger;
    }

    public ValueTask<SignFinalEnvelope> Handle(SignFinalRequest request, CancellationToken cancellationToken)
    {
        this.logger.LogTrace("Entering to Handle with sessionId {SessionId}.", request.SessionId);

        IMemorySession memorySession = this.hwServices.ClientAppCtx.EnsureMemorySession(request.AppId);
        IP11Session p11Session = memorySession.EnsureSession(request.SessionId);

        SignState state = p11Session.State.Ensure<SignState>();

        if (state.RequiredUserLogin && !memorySession.IsUserLogged(p11Session.SlotId))
        {
            throw new RpcPkcs11Exception(CKR.CKR_USER_NOT_LOGGED_IN, "User is not login.");
        }


        byte[] signature = state.GetSignature();

        if (request.IsSignaturePtrSet)
        {
            if (request.PullSignatureLen < (uint)signature.Length)
            {
                return new ValueTask<SignFinalEnvelope>(new SignFinalEnvelope()
                {
                    Rv = (uint)CKR.CKR_BUFFER_TOO_SMALL,
                    Data = null
                });
            }

            p11Session.ClearState();

            return new ValueTask<SignFinalEnvelope>(new SignFinalEnvelope()
            {
                Rv = (uint)CKR.CKR_OK,
                Data = new SignatureData()
                {
                    PullSignatureLen = (uint)signature.Length,
                    Signature = signature
                }
            });
        }
        else
        {
            return new ValueTask<SignFinalEnvelope>(new SignFinalEnvelope()
            {
                Rv = (uint)CKR.CKR_OK,
                Data = new SignatureData()
                {
                    PullSignatureLen = (uint)signature.Length
                }
            });
        }
    }
}