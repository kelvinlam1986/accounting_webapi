using Accounting.Data;
using Accounting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.Repositories
{
    public class ReceiptTypeRepository : IReceiptTypeRepository
    {
        private AccountingContext _context;

        public ReceiptTypeRepository(AccountingContext context)
        {
            this._context = context;
        }

        public bool CheckExistingCode(string code)
        {
            return this._context.ReceiptTypes.Any(x => x.Code == code);
        }

        public bool CheckExistingName(string code, string name)
        {
            return this._context.ReceiptTypes.Any(x => x.ReceiptTypeInVietnamese == name && x.Code != code);
        }

        public IEnumerable<ReceiptType> GetAll(string keyword, int page, int pageSize, out int totalRow)
        {
            totalRow = 0;
            IQueryable<ReceiptType> query = null;
            if (!string.IsNullOrEmpty(keyword))
            {
                query = _context.ReceiptTypes.Where(x => x.Code.Contains(keyword) ||
                    x.ReceiptTypeInVietnamese.Contains(keyword));
            }
            else
            {
                query = _context.ReceiptTypes;
            }

            totalRow = query.Count();
            query = query.OrderBy(x => x.ReceiptTypeInVietnamese)
                        .Skip(page * pageSize)
                        .Take(pageSize);

            return query.ToList();
        }

        public IEnumerable<ReceiptType> GetAllWithoutPaging()
        {
            return this._context.ReceiptTypes.OrderBy(x => x.ReceiptTypeInVietnamese).ToList();
        }

        public ReceiptType GetByCode(string code)
        {
            return this._context.ReceiptTypes.Find(code);
        }

        public bool Insert(ReceiptType receiptType)
        {
            try
            {
                this._context.ReceiptTypes.Add(receiptType);
                int rowEffected = this._context.SaveChanges();
                return rowEffected == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Remove(ReceiptType receiptType)
        {
            try
            {
                this._context.ReceiptTypes.Remove(receiptType);
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
                var receiptType = this._context.ReceiptTypes.Find(code);
                if (receiptType == null)
                {
                    return false;
                }

                this._context.ReceiptTypes.Remove(receiptType);
                int rowEffected = this._context.SaveChanges();
                return rowEffected == 1;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(ReceiptType receiptType)
        {
            try
            {
                this._context.ReceiptTypes.Update(receiptType);
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
