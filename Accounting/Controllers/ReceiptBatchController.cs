using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounting.Infrastructure.Core;
using Accounting.Models;
using Accounting.Repositories;
using Accounting.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.Controllers
{
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/receipt_batch")]
    [Produces("application/json")]
    public class ReceiptBatchController : Controller
    {
        private IReceiptBatchRepository _receiptBatchRepository;
        private IMapper _mapper;

        public ReceiptBatchController(
            IReceiptBatchRepository receiptBatchRepository,
            IMapper mapper)
        {
            this._receiptBatchRepository = receiptBatchRepository;
            this._mapper = mapper;
        }

        [HttpPost("search")]
        [Authorize]
        public IActionResult GetAll([FromBody]SearchDTO searchItem)
        {
            if (searchItem.PageSize == 0)
            {
                searchItem.PageSize = 20;
            }

            int totalRow = 0;
            var receiptBatches = this._receiptBatchRepository.GetAll(
                searchItem.Keyword,
                searchItem.Page,
                searchItem.PageSize, out totalRow);
            var receiptBatchesVm =
                this._mapper.Map<IEnumerable<ReceiptBatch>, 
                    IEnumerable<ReceiptBatchViewModel>>(receiptBatches);
            var pagingVm = new PaginationSet<ReceiptBatchViewModel>();
            pagingVm.Items = receiptBatchesVm;
            pagingVm.Page = searchItem.Page;
            pagingVm.TotalCount = totalRow;
            pagingVm.TotalPage = (int)Math.Ceiling(((decimal)totalRow / searchItem.PageSize));
            pagingVm.MaxPage = pagingVm.TotalPage - 1;
            return Ok(pagingVm);
        }

        [Authorize]
        public IActionResult Post([FromBody]ReceiptBatchAddDTO receiptBatch)
        {
            if (receiptBatch == null)
            {
                return BadRequest(new ErrorViewModel
                {
                    ErrorCode = "400",
                    ErrorMessage = "Thông tin cung cấp không chính xác."
                });
            }

            if (!ModelState.IsValid)
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorCode = "400",
                    ErrorMessage = ModelState.ToErrorMessages()
                };

                return BadRequest(errorViewModel);
            }

            bool isExisting = this._receiptBatchRepository.CheckExistingCode(receiptBatch.ReceiptBatchNo);
            if (isExisting)
            {
                return BadRequest(new ErrorViewModel
                {
                    ErrorCode = "400",
                    ErrorMessage = "Số lô phiếu thu này đã tồn tại."
                });
            }

            var newReceiptBatch = new ReceiptBatch
            {
                ReceiptBatchNo = receiptBatch.ReceiptBatchNo,
                ReceiptBatchDate = receiptBatch.ReceiptBatchDate,
                DescriptionInVietNamese = receiptBatch.DescriptionInVietNamese,
                BatchStatus = receiptBatch.BatchStatus,
                CreatedBy = "admin",
                CreatedDate = DateTime.Now,
                UpdatedBy = "admin",
                UpdatedDate = DateTime.Now
            };

            bool isSuccess = this._receiptBatchRepository.Insert(newReceiptBatch);
            if (isSuccess == false)
            {
                return StatusCode(500, new ErrorViewModel
                {
                    ErrorCode = "500",
                    ErrorMessage = "Có lỗi trong quá trình cập nhật dữ liệu."
                });
            }

            return Ok(newReceiptBatch);
        }

        [HttpPut("")]
        [Authorize]
        public IActionResult Put([FromBody]ReceiptBatchUpdateDTO receiptBatch)
        {
            if (receiptBatch == null)
            {
                return BadRequest(new ErrorViewModel
                {
                    ErrorCode = "400",
                    ErrorMessage = "Thông tin cung cấp không chính xác."
                });
            }

            if (!ModelState.IsValid)
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorCode = "400",
                    ErrorMessage = ModelState.ToErrorMessages()
                };

                return BadRequest(errorViewModel);
            }

            var receiptBatchToUpdate = this._receiptBatchRepository.GetByCode(receiptBatch.ReceiptBatchNo);
            if (receiptBatchToUpdate == null)
            {
                return NotFound(new ErrorViewModel
                {
                    ErrorCode = "404",
                    ErrorMessage = "Số lô phiếu thu cần cập nhật không tìm thấy"
                });
            }

            receiptBatchToUpdate.ReceiptBatchDate = receiptBatch.ReceiptBatchDate;
            receiptBatchToUpdate.DescriptionInVietNamese = receiptBatch.DescriptionInVietNamese;
            receiptBatchToUpdate.BatchStatus = receiptBatch.BatchStatus;
            receiptBatchToUpdate.UpdatedBy = "admin";
            receiptBatchToUpdate.UpdatedDate = DateTime.Now;

            bool isSuccess = this._receiptBatchRepository.Update(receiptBatchToUpdate);
            if (isSuccess == false)
            {
                return StatusCode(500, new ErrorViewModel
                {
                    ErrorCode = "500",
                    ErrorMessage = "Có lỗi trong quá trình cập nhật dữ liệu."
                });
            }

            return Ok(receiptBatchToUpdate);
        }

        [HttpDelete("")]
        [Authorize]
        public IActionResult Delete([FromBody]ReceiptBatchDeleteDTO receiptBatch)
        {
            if (receiptBatch == null)
            {
                return BadRequest(new ErrorViewModel
                {
                    ErrorCode = "400",
                    ErrorMessage = "Thông tin cung cấp không chính xác."
                });
            }

            if (!ModelState.IsValid)
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorCode = "400",
                    ErrorMessage = ModelState.ToErrorMessages()
                };

                return BadRequest(errorViewModel);
            }

            var receiptBatchToDelete = this._receiptBatchRepository.GetByCode(receiptBatch.ReceiptBatchNo);
            if (receiptBatchToDelete == null)
            {
                return NotFound(new ErrorViewModel
                {
                    ErrorCode = "404",
                    ErrorMessage = "Loại phiếu thu cần xóa không tìm thấy"
                });
            }

            bool isSuccess = this._receiptBatchRepository.Remove(receiptBatchToDelete);
            if (isSuccess == false)
            {
                return StatusCode(500, new ErrorViewModel
                {
                    ErrorCode = "500",
                    ErrorMessage = "Có lỗi trong quá trình cập nhật dữ liệu."
                });
            }

            return Ok(receiptBatchToDelete);
        }
    }
}