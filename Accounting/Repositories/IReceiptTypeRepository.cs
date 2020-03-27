using Accounting.Models;
using System.Collections.Generic;

namespace Accounting.Repositories
{
    public interface IReceiptTypeRepository
    {
        IEnumerable<ReceiptType> GetAll(string keyword, int page, int pageSize, out int totalRow);
        ReceiptType GetByCode(string code);
        bool Update(ReceiptType receiptType);
        bool Insert(ReceiptType receiptType);
        bool Remove(ReceiptType receiptType);
        bool RemoveByCode(string code);
        bool CheckExistingName(string code, string name);
        bool CheckExistingCode(string code);
        IEnumerable<ReceiptType> GetAllWithoutPaging();
    }
}
