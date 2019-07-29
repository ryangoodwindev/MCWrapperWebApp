using MCWrapper.Data;
using MCWrapper.MachineShop;
using MCWrapper.MultiChainCLI;
using MCWrapper.MultiChainRPC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace MCWrapperWebApp.Pages
{
    public class IndexModel : PageModel
    {
        // Forge services allow consumers to start and stop multichaind.exe
        private readonly IForge _forge;

        // Blockchain Command Line Interface (CLI) services allow the 
        // consumer to send commands directly to multichain-cli.exe
        private readonly IBlockchainCLI _cli;

        // Blockchain JSON-RPC (RPC) services allow the 
        // consumer to send commands to a blockchain over HTTP/HTTPS connections.
        // 
        // For this scenario we are using HTTP connections as dictated by the 
        // BlockchainProfileOptions property 'ChainUseSsl' being set as false.
        private readonly IBlockchainRPC _rpc;

        // Import Ioptions pattern
        private readonly BlockchainProfileOptions _options;

        /// <summary>
        /// Create a new IndexModel instance with parameters
        /// </summary>
        /// <param name="forge"></param>
        /// <param name="cli"></param>
        /// <param name="rpc"></param>
        /// <param name="options"></param>
        public IndexModel(
            IForge forge,
            IBlockchainCLI cli, 
            IBlockchainRPC rpc,
            IOptions<BlockchainProfileOptions> options)
        {
            // assign Command Line services
            _cli = cli;

            // assign JSON-RPC services
            _rpc = rpc;

            // assign Forge services
            _forge = forge;

            // assign IOptions pattern containing BlockchainProfileOptions
            _options = options.Value;
        }

        /// <summary>
        /// Last block information model
        /// </summary>
        public GetLastBlockInfoResult BlockInfo { get; set; }

        /// <summary>
        /// Blockchain information model
        /// </summary>
        public GetBlockchainInfoResult BlockchainInfo { get; set; }

        /// <summary>
        /// GET /Index
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync()
        {
            // Optional - Not necessary if blockchain already exists and has been started, Uncomment for use.
            //
            // If the target blockchain is not started or does not exist then Forge services will ensure
            // the target blockchain is created and has been started before proceeding.
            //
            //var tryCreate = await _forge.CreateBlockchainAsync(_options.ChainName);
            //if (!tryCreate.Success)
            //    return BadRequest($"Unable to create blokchain named, {_options.ChainName}");
            //await _forge.StartBlockchainAsync();

            // Example JSON-RPC call over HTTP;
            // Fetches target blockchain information
            RPCClientResponse<GetBlockchainInfoResult> blockchain = await _rpc.GetBlockchainInfoAsync();
            BlockchainInfo = blockchain.Result;

            // Example CLI call that interacts directly with multichain-cli.exe versus using JSON-RPC over HTTP
            CLIClientResponse<GetLastBlockInfoResult> blockInfo = await _cli.GetLastBlockInfoAsync();
            BlockInfo = blockInfo.Result;

            return Page();
        }
    }
}
