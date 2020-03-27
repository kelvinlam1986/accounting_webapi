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
    [Route("/api/v{version:apiVersion}/receipt_type")]
    [Produces("application/json")]
    public class ReceiptTypeController : Controller
    {
        private IReceiptTypeRepository _receiptTypeRepository;
        private IMapper _mapper;

        public ReceiptTypeController(
            IReceiptTypeRepository receiptTypeRepository,
            IMapper mapper)
        {
            this._receiptTypeRepository = receiptTypeRepository;
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
            var receiptTypes = this._receiptTypeRepository.GetAll(
                searchItem.Keyword,
                searchItem.Page,
                searchItem.PageSize, out totalRow);
            var receiptTypesVm =
                this._mapper.Map<IEnumerable<ReceiptType>, IEnumerable<ReceiptTypeViewModel>>(receiptTypes);
            var pagingVm = new PaginationSet<ReceiptTypeViewModel>();
            pagingVm.Items = receiptTypesVm;
            pagingVm.Page = searchItem.Page;
            pagingVm.TotalCount = totalRow;
            pagingVm.TotalPage = (int)Math.Ceiling(((decimal)totalRow / searchItem.PageSize));
            pagingVm.MaxPage = pagingVm.TotalPage - 1;
            return Ok(pagingVm);
        }

        [HttpPost("")]
        [Authorize]
        public IActionResult Post([FromBody]ReceiptTypeAddDTO receiptType)
        {
            if (receiptType == null)
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

            bool isExisting = this._receiptTypeRepository.CheckExistingCode(receiptType.Code);
            if (isExisting)
            {
                return BadRequest(new ErrorViewModel
                {
                    ErrorCode = "400",
                    ErrorMessage = "Mã loại phiếu thu này đã tồn tại."
                });
            }

            isExisting = this._receiptTypeRepository.CheckExistingName("", receiptType.ReceiptTypeInVietnamese);
            if (isExisting)
            {
                return BadRequest(new ErrorViewModel
                {
                    ErrorCode = "400",
                    ErrorMessage = "Tên loại phiếu thu này đã tồn tại."
                });

            }

            var newReceiptType = new ReceiptType
            {
                Code = receiptType.Code,
                ReceiptTypeInVietnamese = receiptType.ReceiptTypeInVietnamese,
                ReceiptTypeInSecondLanguage = receiptType.ReceiptTypeInSecondLanguage,
                ShowReceiptTypeInVietNamese= receiptType.ShowReceiptTypeInVietNamese,
                CreatedBy = "admin",
                CreatedDate = DateTime.Now,
                UpdatedBy = "admin",
                UpdatedDate = DateTime.Now
            };

            bool isSuccess = this._receiptTypeRepository.Insert(newReceiptType);
            if (isSuccess == false)
            {
                return StatusCode(500, new ErrorViewModel
                {
                    ErrorCode = "500",
                    ErrorMessage = "Có lỗi trong quá trình cập nhật dữ liệu."
                });
            }

            return Ok(newReceiptType);
        }

        [HttpPut("")]
        [Authorize]
        public IActionResult Put([FromBody]ReceiptTypeUpdateDTO receiptType)
        {
            if (receiptType == null)
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

            var receiptTypeToUpdate = this._receiptTypeRepository.GetByCode(receiptType.Code);
            if (receiptTypeToUpdate == null)
            {
                return NotFound(new ErrorViewModel
                {
                    ErrorCode = "404",
                    ErrorMessage = "Loại phiếu thu cần cập nhật không tìm thấy"
                });
            }

            bool isExisting = this._receiptTypeRepository.CheckExistingName(
                receiptType.Code, receiptType.ReceiptTypeInVietnamese);
            if (isExisting)
            {
                return BadRequest(new ErrorViewModel
                {
                    ErrorCode = "400",
                    ErrorMessage = "Tên loại phiếu thu này đã tồn tại."
                });
            }

            receiptTypeToUpdate.ReceiptTypeInVietnamese = receiptType.ReceiptTypeInVietnamese;
            receiptTypeToUpdate.ReceiptTypeInSecondLanguage = receiptType.ReceiptTypeInSecondLanguage;
            receiptTypeToUpdate.ShowReceiptTypeInVietNamese = receiptType.ShowReceiptTypeInVietNamese;
            receiptTypeToUpdate.UpdatedBy = "admin";
            receiptTypeToUpdate.UpdatedDate = DateTime.Now;

            bool isSuccess = this._receiptTypeRepository.Update(receiptTypeToUpdate);
            if (isSuccess == false)
            {
                return StatusCode(500, new ErrorViewModel
                {
                    ErrorCode = "500",
                    ErrorMessage = "Có lỗi trong quá trình cập nhật dữ liệu."
                });
            }

            return Ok(receiptTypeToUpdate);
        }

        [HttpDelete("")]
        [Authorize]
        public IActionResult Delete([FromBody]ReceiptTypeDeleteDTO receiptType)
        {
            if (receiptType == null)
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

            var receiptTypeToDelete = this._receiptTypeRepository.GetByCode(receiptType.Code);
            if (receiptTypeToDelete == null)
            {
                return NotFound(new ErrorViewModel
                {
                    ErrorCode = "404",
                    ErrorMessage = "Loại phiếu thu cần xóa không tìm thấy"
                });
            }

            bool isSuccess = this._receiptTypeRepository.Remove(receiptTypeToDelete);
            if (isSuccess == false)
            {
                return StatusCode(500, new ErrorViewModel
                {
                    ErrorCode = "500",
                    ErrorMessage = "Có lỗi trong quá trình cập nhật dữ liệu."
                });
            }

            return Ok(receiptTypeToDelete);
        }
    }
}