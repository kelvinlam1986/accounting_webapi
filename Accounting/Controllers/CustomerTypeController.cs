using Accounting.Infrastructure.Core;
using Accounting.Models;
using Accounting.Repositories;
using Accounting.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Accounting.Controllers
{
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/customer_type")]
    [Produces("application/json")]
    public class CustomerTypeController : Controller
    {
        private ICustomerTypeRepository _customerTypeRepository;
        private IMapper _mapper;

        public CustomerTypeController(
            ICustomerTypeRepository customerTypeRepository,
            IMapper mapper)
        {
            this._customerTypeRepository = customerTypeRepository;
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
            var customerTypes = this._customerTypeRepository.GetAll(
                searchItem.Keyword,
                searchItem.Page,
                searchItem.PageSize, out totalRow);
            var customerTypesVm =
                this._mapper.Map<IEnumerable<CustomerType>, IEnumerable<CustomerTypeViewModel>>(customerTypes);
            var pagingVm = new PaginationSet<CustomerTypeViewModel>();
            pagingVm.Items = customerTypesVm;
            pagingVm.Page = searchItem.Page;
            pagingVm.TotalCount = totalRow;
            pagingVm.TotalPage = (int)Math.Ceiling(((decimal)totalRow / searchItem.PageSize));
            pagingVm.MaxPage = pagingVm.TotalPage - 1;
            return Ok(pagingVm);
        }

        [HttpPost("")]
        [Authorize]
        public IActionResult Post([FromBody]CustomerTypeAddDTO customerType)
        {
            if (customerType == null)
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

            bool isExisting = this._customerTypeRepository.CheckExistingCode(customerType.Code);
            if (isExisting)
            {
                return BadRequest(new ErrorViewModel
                {
                    ErrorCode = "400",
                    ErrorMessage = "Mã loại khách hàng này đã tồn tại."
                });
            }

            isExisting = this._customerTypeRepository.CheckExistingName("", customerType.Name);
            if (isExisting)
            {
                return BadRequest(new ErrorViewModel
                {
                    ErrorCode = "400",
                    ErrorMessage = "Tên loại khách hàng này đã tồn tại."
                });

            }

            var newCustomerType = new CustomerType
            {
                Code = customerType.Code,
                Name = customerType.Name,
                CreatedBy = "admin",
                CreatedDate = DateTime.Now,
                UpdatedBy = "admin",
                UpdatedDate = DateTime.Now
            };

            bool isSuccess = this._customerTypeRepository.Insert(newCustomerType);
            if (isSuccess == false)
            {
                return StatusCode(500, new ErrorViewModel
                {
                    ErrorCode = "500",
                    ErrorMessage = "Có lỗi trong quá trình cập nhật dữ liệu."
                });
            }

            return Ok(newCustomerType);
        }

        [HttpPut("")]
        [Authorize]
        public IActionResult Put([FromBody]CustomerTypeUpdateDTO customerType)
        {
            if (customerType == null)
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

            var customerTypeToUpdate = this._customerTypeRepository.GetByCode(customerType.Code);
            if (customerTypeToUpdate == null)
            {
                return NotFound(new ErrorViewModel
                {
                    ErrorCode = "404",
                    ErrorMessage = "Loại khách hàng cần cập nhật không tìm thấy"
                });
            }

            bool isExisting = this._customerTypeRepository.CheckExistingName(
                customerType.Code, customerType.Name);
            if (isExisting)
            {
                return BadRequest(new ErrorViewModel
                {
                    ErrorCode = "400",
                    ErrorMessage = "Tên loại khách hàng này đã tồn tại."
                });
            }

            customerTypeToUpdate.Name = customerType.Name;
            customerTypeToUpdate.UpdatedBy = "admin";
            customerTypeToUpdate.UpdatedDate = DateTime.Now;

            bool isSuccess = this._customerTypeRepository.Update(customerTypeToUpdate);
            if (isSuccess == false)
            {
                return StatusCode(500, new ErrorViewModel
                {
                    ErrorCode = "500",
                    ErrorMessage = "Có lỗi trong quá trình cập nhật dữ liệu."
                });
            }

            return Ok(customerTypeToUpdate);
        }

        [HttpDelete("")]
        [Authorize]
        public IActionResult Delete([FromBody]CustomerTypeDeleteDTO customerType)
        {
            if (customerType == null)
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

            var customerTypeToDelete = this._customerTypeRepository.GetByCode(customerType.Code);
            if (customerTypeToDelete == null)
            {
                return NotFound(new ErrorViewModel
                {
                    ErrorCode = "404",
                    ErrorMessage = "Loại khách hàng cần xóa không tìm thấy"
                });
            }

            bool isSuccess = this._customerTypeRepository.Remove(customerTypeToDelete);
            if (isSuccess == false)
            {
                return StatusCode(500, new ErrorViewModel
                {
                    ErrorCode = "500",
                    ErrorMessage = "Có lỗi trong quá trình cập nhật dữ liệu."
                });
            }

            return Ok(customerTypeToDelete);
        }
    }
}