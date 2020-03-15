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
    [Route("/api/v{version:apiVersion}/bank")]
    [Produces("application/json")]
    public class BankController : Controller
    {
        private IBankRepository _bankRepository;
        private IMapper _mapper;

        public BankController(
            IBankRepository bankRepository,
            IMapper mapper)
        {
            this._bankRepository = bankRepository;
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
            var banks = this._bankRepository.GetAll(
                searchItem.Keyword,
                searchItem.Page,
                searchItem.PageSize, out totalRow);
            var banksVm =
                this._mapper.Map<IEnumerable<Bank>, IEnumerable<BankViewModel>>(banks);
            var pagingVm = new PaginationSet<BankViewModel>();
            pagingVm.Items = banksVm;
            pagingVm.Page = searchItem.Page;
            pagingVm.TotalCount = totalRow;
            pagingVm.TotalPage = (int)Math.Ceiling(((decimal)totalRow / searchItem.PageSize));
            pagingVm.MaxPage = pagingVm.TotalPage - 1;
            return Ok(pagingVm);
        }
    }
}