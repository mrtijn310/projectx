using Crouny.Models;

namespace Crouny.DAL.Interfaces
{
    public interface IAccountRepository
    {
        AccountModel GetAccount(int accountId); 
    }
}
