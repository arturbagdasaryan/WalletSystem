using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace WalletSystem.Api.Controllers
{
    [RoutePrefix("api/wallet")]
    public class WalletController : ApiController
    {
        private readonly WalletService _walletService;
        public WalletController(WalletService walletService) => _walletService = walletService;

        [HttpPost, Route("create")]
        public async Task<IHttpActionResult> Create() => Ok(await _walletService.CreateWalletAsync());

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> Get(Guid id) => Ok(await _walletService.GetBalanceAsync(id));

        [HttpPost, Route("add")]
        public async Task<IHttpActionResult> Add(WalletActionRequest req)
        {
            await _walletService.AddFundsAsync(req.WalletId, req.Amount, req.TransactionId);
            return Ok();
        }

        [HttpPost, Route("remove")]
        public async Task<IHttpActionResult> Remove(WalletActionRequest req)
        {
            await _walletService.RemoveFundsAsync(req.WalletId, req.Amount, req.TransactionId);
            return Ok();
        }

        public class WalletActionRequest
        {
            public Guid WalletId { get; set; }
            public decimal Amount { get; set; }
            public Guid TransactionId { get; set; }
        }
    }
}