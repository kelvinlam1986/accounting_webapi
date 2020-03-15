using Accounting.Models;
using System.Collections.Generic;

namespace Accounting.Repositories
{
    public interface ICustomerTypeRepository
    {
        IEnumerable<CustomerType> GetAll(string keyword, int page, int pageSize, out int totalRow);
        CustomerType GetByCode(string code);
        bool Update(CustomerType customerType);
        bool Insert(CustomerType customerType);
        bool Remove(CustomerType customerType);
        bool RemoveByCode(string code);
        bool CheckExistingName(string code, string name);
        bool CheckExistingCode(string code);
        IEnumerable<CustomerType> GetAllWithoutPaging();
    }
}
