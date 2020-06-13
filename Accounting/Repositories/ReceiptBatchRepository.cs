using System;
using System.Collections.Generic;
using System.Linq;
using Accounting.Data;
using Accounting.Models;

namespace Accounting.Repositories
{
    public class ReceiptBatchRepository : IReceiptBatchRepository
    {
        private AccountingContext _context;

        public ReceiptBatchRepository(AccountingContext context)
        {
            this._context = context;
        }

        public bool CheckExistingCode(string batchNo)
        {
            return this._context.ReceiptBatches.Any(x => x.ReceiptBatchNo == batchNo);
        }

        public IEnumerable<ReceiptBatch> GetAll(string keyword, int page, int pageSize, out int totalRow)
        {
            totalRow = 0;
            IQueryable<ReceiptBatch> query = null;
            if (!string.IsNullOrEmpty(keyword))
            {
                query = _context.ReceiptBatches.Where(x => x.ReceiptBatchNo.Contains(keyword) ||
                    x.DescriptionInVietNamese.Contains(keyword));
            }
            else
            {
                query = _context.ReceiptBatches;
            }

            totalRow = query.Count();
            query = query.OrderBy(x => x.ReceiptBatchNo)
                        .Skip(page * pageSize)
                        .Take(pageSize);

            return query.ToList();
        }

        public IEnumerable<ReceiptBatch> GetAllWithoutPaging()
        {
            return this._context.ReceiptBatches.OrderBy(x => x.ReceiptBatchNo).ToList();
        }

        public ReceiptBatch GetByCode(string batchNo)
        {
            return this._context.ReceiptBatches.Find(batchNo);
        }

        public bool Insert(ReceiptBatch receiptBatch)
        {
            try
            {
                this._context.ReceiptBatches.Add(receiptBatch);
                int rowEffected = this._context.SaveChanges();
                return rowEffected == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Remove(ReceiptBatch receiptBatch)
        {
            try
            {
                this._context.ReceiptBatches.Remove(receiptBatch);
                int rowEffected = this._context.SaveChanges();
                return rowEffected == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveByCode(string code)
        {
            try
            {
                var receiptBatch = this._context.ReceiptBatches.Find(code);
                if (receiptBatch == null)
                {
                    return false;
                }

                this._context.ReceiptBatches.Remove(receiptBatch);
                int rowEffected = this._context.SaveChanges();
                return rowEffected == 1;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(ReceiptBatch receiptBatch)
        {
            try
            {
                this._context.ReceiptBatches.Update(receiptBatch);
                int rowEffected = this._context.SaveChanges();
                return rowEffected == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
