using MCWrapper.CLI.Ledger.Clients;
using MCWrapper.Data.Models.Blockchain;
using MCWrapper.RPC.Ledger.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MCWrapperWebApp.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        // Blockchain JSON-RPC (RPC) services allow the 
        // consumer to send commands to a blockchain over HTTP/HTTPS connections.
        // 
        // For this scenario we are using HTTP connections as dictated by the 
        // BlockchainProfileOptions property 'ChainUseSsl' being set as false.
        private readonly IMultiChainRpcGeneral _generalRpc;
        private readonly IMultiChainCliGeneral _generalCli;

        /// <summary>
        /// Create a new IndexModel instance with parameters
        /// </summary>
        /// <param name="generalRpc"></param>
        /// <param name="options"></param>
        public IndexModel(IMultiChainRpcGeneral generalRpc,
            IMultiChainCliGeneral generalCli)
        {
            _generalRpc = generalRpc;
            _generalCli = generalCli;
        }

        /// <summary>
        /// Blockchain information model
        /// </summary>
        public GetBlockchainInfoResult BlockchainInfo { get; private set; }

        public GetStreamInfoResult StreamInfo { get; private set; }

        /// <summary>
        /// GET /Index
        /// </summary>
        /// <returns></returns>
        public async Task OnGetAsync()
        {
            // Example JSON-RPC call over HTTP;
            // Fetches target blockchain information
            var blockchainInfo = await _generalRpc.GetBlockchainInfoAsync();
            var streamInfo = await _generalCli.GetStreamInfoAsync("root", true);

            BlockchainInfo = blockchainInfo.Result;
            StreamInfo = streamInfo.Result;
        }
    }
}