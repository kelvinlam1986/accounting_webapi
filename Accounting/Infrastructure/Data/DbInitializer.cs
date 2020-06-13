using Accounting.Data;
using Accounting.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Accounting.Infrastructure.Data
{
    public class DbInitializer
    {
        public static void RecreateDatabase(AccountingContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        public static void Initialize(AccountingContext context)
        {
            var email = "kelvincoder@gmail.com";
            var adminRoleId = string.Empty;
            var userId = string.Empty;

            if (context.Users.Any(r => r.Email.Equals(email)))
                userId = context.Users.First(r => r.Email.Equals(email)).Id;

            if (!userId.Equals(string.Empty))
            {
                if (!context.Banks.Any())
                {
                    var banks = new List<Bank>
                    {
                        new Bank {
                            Code = "001",
                            Name = "Ngân hàng Đông Á",
                            Address = "120 Lý Tự Trọng P. Bến Thành Q1 TP.HCM",
                            CreatedBy = "admin",
                            CreatedDate = new DateTime(2019, 12, 8, 12, 0, 0),
                            UpdatedBy = "admin",
                            UpdatedDate = new DateTime(2019, 12, 8, 12, 0, 0)
                        },
                        new Bank {
                            Code = "002",
                            Name = "Ngân hàng BIDV",
                            Address = "140 Trần Hưng Đạo P. Bến Nghé Q1 TP.HCM",
                            CreatedBy = "admin",
                            CreatedDate = new DateTime(2019, 12, 8, 12, 0, 0),
                            UpdatedBy = "admin",
                            UpdatedDate = new DateTime(2019, 12, 8, 12, 0, 0)
                        }
                    };
                    context.Banks.AddRange(banks);
                    context.SaveChanges();
                }
            }

            if (!context.CustomerTypes.Any())
            {
                var customerTypes = new List<CustomerType>
                    {
                        new CustomerType {
                            Code = "001",
                            Name = "Khách hàng công ty",
                            CreatedBy = "admin",
                            CreatedDate = new DateTime(2019, 12, 8, 12, 0, 0),
                            UpdatedBy = "admin",
                            UpdatedDate = new DateTime(2019, 12, 8, 12, 0, 0)
                        },
                        new CustomerType {
                            Code = "002",
                            Name = "Khách hàng lẻ",
                            CreatedBy = "admin",
                            CreatedDate = new DateTime(2019, 12, 8, 12, 0, 0),
                            UpdatedBy = "admin",
                            UpdatedDate = new DateTime(2019, 12, 8, 12, 0, 0)
                        }
                    };
                context.CustomerTypes.AddRange(customerTypes);
                context.SaveChanges();
            }

            if (!context.ReceiptTypes.Any())
            {
                var receiptTypes = new List<ReceiptType>
                    {
                        new ReceiptType {
                            Code = "001",
                            ReceiptTypeInVietnamese = "Thu tiền nợ khách hàng",
                            ReceiptTypeInSecondLanguage = "Customer Receipt",
                            ShowReceiptTypeInVietNamese = true,
                            CreatedBy = "admin",
                            CreatedDate = new DateTime(2019, 12, 8, 12, 0, 0),
                            UpdatedBy = "admin",
                            UpdatedDate = new DateTime(2019, 12, 8, 12, 0, 0)
                        },
                        new ReceiptType {
                            Code = "002",
                            ReceiptTypeInVietnamese = "Thu tiền nội bộ",
                            ReceiptTypeInSecondLanguage = "Internal Receipt",
                            ShowReceiptTypeInVietNamese = true,
                            CreatedBy = "admin",
                            CreatedDate = new DateTime(2019, 12, 8, 12, 0, 0),
                            UpdatedBy = "admin",
                            UpdatedDate = new DateTime(2019, 12, 8, 12, 0, 0)
                        }
                    };
                context.ReceiptTypes.AddRange(receiptTypes);
                context.SaveChanges();
            }
        }
    }
}
