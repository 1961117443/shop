using System.Threading.Tasks;

namespace Shop.IService
{
    public interface IDataTranService
    {
        Task<bool> Tran();
        Task<string> StructureSQL();
    }
}