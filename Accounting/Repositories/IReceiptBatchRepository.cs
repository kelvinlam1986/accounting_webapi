using Accounting.Models;
using System.Collections.Generic;

namespace Accounting.Repositories
{
    public interface IReceiptBatchRepository
    {
        IEnumerable<ReceiptBatch> GetAll(string keyword, int page, int pageSize, out int totalRow);
        ReceiptBatch GetByCode(string code);
        bool Update(ReceiptBatch receiptBatch);
        bool Insert(ReceiptBatch receiptBatch);
        bool Remove(ReceiptBatch receiptBatchs);
        bool RemoveByCode(string code);
        bool CheckExistingCode(string code);
        IEnumerable<ReceiptBatch> GetAllWithoutPaging();
    }
}
