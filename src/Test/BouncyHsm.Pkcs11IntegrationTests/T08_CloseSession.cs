﻿using Net.Pkcs11Interop.HighLevelAPI;
using Net.Pkcs11Interop.Common;

namespace BouncyHsm.Pkcs11IntegrationTests;

[TestClass]
public class T08_CloseSession
{
    [TestMethod]
    public void CloseSession_Call_Success()
    {
        Pkcs11InteropFactories factories = new Pkcs11InteropFactories();
        using IPkcs11Library library = factories.Pkcs11LibraryFactory.LoadPkcs11Library(factories,
            AssemblyTestConstants.P11LibPath,
            AppType.SingleThreaded);

        List<ISlot> slots = library.GetSlotList(SlotsType.WithTokenPresent);
        ISlot slot = slots.SelectTestSlot();

        ISession session = slot.OpenSession(SessionType.ReadOnly);

        session.CloseSession();
    }
}
