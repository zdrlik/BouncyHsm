using BouncyHsm.Nuget.Example;
using Monet.BouncyHsm;
using Net.Pkcs11Interop.Common;

var bouncyConfig = Environment.GetEnvironmentVariable("BOUNCY_HSM_CFG_STRING");
var libraryPath = BouncyHsmExtensions.GetPkcs11LibraryPath();
Console.WriteLine($"BOUNCY_HSM_CFG_STRING: {bouncyConfig}");
Console.WriteLine($"Library path: {libraryPath}");
try
{
    CryptoService cryptoService = new(libraryPath);
    cryptoService.InitializePkcs11Library();
    var slotList = cryptoService.GetSlotList(SlotsType.WithOrWithoutTokenPresent);
    if (slotList.Count == 0)
    {
        Console.WriteLine("No slots found.");
    }
    else
    {
        Console.WriteLine("Slots found:");
        foreach (var slot in slotList)
        {
            Console.WriteLine($"Slot: {slot.SlotId}");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
finally
{
    BouncyHsmExtensions.RemoveBouncyHsmConnectionString();
}
