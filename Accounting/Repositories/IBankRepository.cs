using Accounting.Models;
using System.Collections.Generic;

namespace Accounting.Repositories
{
    public interface IBankRepository
    {
        IEnumerable<Bank> GetAll(string keyword, int page, int pageSize, out int totalRow);
        Bank GetByCode(string code);
        bool Update(Bank bank);
        bool Insert(Bank bank);
        bool Remove(Bank bank);
        bool RemoveByCode(string code);
        bool CheckExistingName(string code, string name);
        bool CheckExistingCode(string code);
        IEnumerable<Bank> GetAllWithoutPaging();
    }
}
