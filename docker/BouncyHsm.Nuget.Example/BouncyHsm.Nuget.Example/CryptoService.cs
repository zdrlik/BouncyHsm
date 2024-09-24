using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.HighLevelAPI;
using Net.Pkcs11Interop.HighLevelAPI.Factories;

namespace BouncyHsm.Nuget.Example;
public class CryptoService
{
    private readonly string cryptoServiceLibraryPath;
    private IPkcs11Library? pkcs11Library;

    public CryptoService(string cryptoServiceLibraryPath)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(cryptoServiceLibraryPath);

        this.cryptoServiceLibraryPath = cryptoServiceLibraryPath;
    }

    public void InitializePkcs11Library()
    {
        try
        {
            var pkcs11Factory = new Pkcs11LibraryFactory();
            pkcs11Library = pkcs11Factory.LoadPkcs11Library(
                new Pkcs11InteropFactories(),
                cryptoServiceLibraryPath,
                AppType.MultiThreaded,
                InitType.WithFunctionList);
        }
        catch (Pkcs11Exception ex)
        {
            throw new Pkcs11Exception($"Failed to initialize PKCS11 library due to error {ex.InnerException} and return value {ex.RV}", ex.RV);
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Failed to initialize PKCS11 library: {ex.Message}", ex);
        }
    }

    public IList<ISlot> GetSlotList(SlotsType slotsType)
    {
        if (pkcs11Library == null)
        {
            throw new InvalidOperationException("PKCS11 library is not initialized.");
        }

        try
        {
            return pkcs11Library.GetSlotList(slotsType);
        }
        catch (Pkcs11Exception ex)
        {
            throw new Pkcs11Exception($"Failed to get slot list due to error {ex.InnerException} and return value {ex.RV}", ex.RV);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to get slot list: {ex.Message}", ex);
        }
    }
}
