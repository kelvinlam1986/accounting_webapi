using Microsoft.AspNetCore.Mvc;

namespace Accounting.Controllers
{
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/customer_type")]
    [Produces("application/json")]
    public class CustomerTypeController : Controller
    {

    }
}