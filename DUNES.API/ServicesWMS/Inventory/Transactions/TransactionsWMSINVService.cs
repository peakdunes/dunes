using DUNES.API.Models.Masters;
using DUNES.API.ModelsWMS.Masters;
using DUNES.API.Repositories.Inventory.PickProcess.Transactions;
using DUNES.API.Repositories.Masters;
using DUNES.API.RepositoriesWMS.Inventory.Common.Queries;
using DUNES.API.RepositoriesWMS.Inventory.Transactions;
using DUNES.API.Services.Masters;
using DUNES.API.ServicesWMS.Inventory.Common.Queries;
using DUNES.API.ServicesWMS.Masters;
using DUNES.API.Utils.Responses;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;

namespace DUNES.API.ServicesWMS.Inventory.Transactions
{
    /// <summary>
    /// All WMS inventory transactions
    /// </summary>
    public class TransactionsWMSINVService : ITransactionsWMSINVService
    {

        private readonly ICommonQueryWMSMasterService _commonQueryWMSMasterService;
        private readonly IMasterService<WmsCompanyclient> _masterCompanyclientService;
        private readonly IMasterService<TdivisionCompany> _masterCompanyclientDivisionService;

        private readonly IMasterService<TzebB2bMasterPartDefinition> _masterCompanyclientMasterInventory;

        private readonly ICommonQueryWMSINVService _commonQueryWMSService;
        private readonly ITransactionsWMSINVRepository _repository;

        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="commonQueryWMSService"></param>
        /// <param name="masterCompanyclientService"></param>
        /// <param name="commonQueryWMSMasterService"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="masterCompanyclientDivisionService"></param>
        /// <param name="masterCompanyclientMasterInventory"></param>
        public TransactionsWMSINVService(ITransactionsWMSINVRepository repository, 
            ICommonQueryWMSINVService commonQueryWMSService, IMasterService<WmsCompanyclient> masterCompanyclientService,
            ICommonQueryWMSMasterService commonQueryWMSMasterService, IHttpContextAccessor httpContextAccessor,
            IMasterService<TdivisionCompany> masterCompanyclientDivisionService, IMasterService<TzebB2bMasterPartDefinition> masterCompanyclientMasterInventory)
        {
            _repository = repository;
            _commonQueryWMSService = commonQueryWMSService;
            _masterCompanyclientService = masterCompanyclientService;
            _commonQueryWMSMasterService = commonQueryWMSMasterService;
            _httpContextAccessor = httpContextAccessor;
            _masterCompanyclientDivisionService = masterCompanyclientDivisionService;
            _masterCompanyclientMasterInventory = masterCompanyclientMasterInventory;
        }

        /// <summary>
        /// create a new inventory transaction
        /// </summary>
        /// <param name="objcreate"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<int>> CreateInventoryTransaction(NewInventoryTransactionTm objcreate)
        {

            bool isSerialized = false;
            bool thereIsIdConcept = false;
            bool thereIsDivision = false;
            bool thereIsLocation = false;


            bool thereIsInventoryType = false;
            bool thereIsItemStatus = false;
            bool thereIsRacks = false;

            //null validation
            if (objcreate == null)
                return ApiResponseFactory.BadRequest<int>("Invalid request data");

            //HDR data validation
            if (objcreate.hdr.Idcompany <= 0)
                return ApiResponseFactory.BadRequest<int>("Company information is required");
            
            //company id validation
            var infocompany = await _commonQueryWMSMasterService.GetCompanyInformation(objcreate.hdr.Idcompany);

            if(infocompany.Data == null)
                return ApiResponseFactory.BadRequest<int>("Company  not found");

            //company client validation
            if (string.IsNullOrEmpty(objcreate.hdr.Codecompanyclient))
                return ApiResponseFactory.BadRequest<int>("Company Client is required");

            var infocompanyclient = await _masterCompanyclientService.GetByIdAsync(objcreate.hdr.Idcompanyclient);
            
            if (infocompanyclient.Data == null)
                return ApiResponseFactory.BadRequest<int>("Company Client information not found");


            // concept validation

            if (objcreate.hdr.Idtransactionconcept <= 0)
                return ApiResponseFactory.BadRequest<int>("Concept Transaction is required");

            var infoconcepts = await _commonQueryWMSService.GetAllTransactionsConcept(objcreate.hdr.Idcompany, objcreate.hdr.Codecompanyclient);

            if(infoconcepts.Data == null)
                return ApiResponseFactory.BadRequest<int>("Concept Transaction not found");

            foreach(var info in  infoconcepts.Data)
            {
                if (info.Id == objcreate.hdr.Idtransactionconcept)
                {
                    thereIsIdConcept = true;
                }
            }

            if (thereIsIdConcept)
            {
                return ApiResponseFactory.BadRequest<int>("Concept Transaction not valid");
            }

            //user validation

            if (string.IsNullOrEmpty(objcreate.hdr.IdUser))
                return ApiResponseFactory.BadRequest<int>("User not found");

            //we obtain JWT user authenticated

            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("name")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return ApiResponseFactory.Unauthorized<int>("User not authenticated");

                       
            if (objcreate.hdr.IdUser != userIdClaim)
                return ApiResponseFactory.Forbidden<int>("User ID in payload does not match authenticated user");



            //division validation
            if (string.IsNullOrEmpty(objcreate.hdr.Iddivision))
                return ApiResponseFactory.BadRequest<int>("Client division is required");

            var infodivision = await _masterCompanyclientDivisionService.SearchByFieldAsync("CompanyDsc", objcreate.hdr.Codecompanyclient);

            if (infodivision.Data == null)
                return ApiResponseFactory.BadRequest<int>("Client division not found");

            foreach(var infodiv in infodivision.Data)
            {
                if (infodiv.DivisionDsc.ToUpper() == objcreate.hdr.Iddivision.ToUpper() && infodiv.Active == true)
                {
                    thereIsDivision = true;
                    break;
                }
            }

            if(!thereIsDivision)
            {
                return ApiResponseFactory.BadRequest<int>($"This division : {objcreate.hdr.Iddivision.Trim()} was not found or there is not active");
            }


            //list detail validation
            if (objcreate.Listdetails == null || !objcreate.Listdetails.Any())
                return ApiResponseFactory.BadRequest<int>("Transaction must contain at least one detail");

            //Validation detail

            foreach (var info in objcreate.Listdetails)
            {


                //location validation

                if (info.Idlocation <= 0)
                    return ApiResponseFactory.BadRequest<int>("Location not found");

                var infoloc = await _commonQueryWMSMasterService.GetAllLocationsByCompany(objcreate.hdr.Idcompany);

                if (infoloc.Data.Count <= 0)
                    return ApiResponseFactory.BadRequest<int>($"Locations for this company {objcreate.hdr.Idcompany} not found");

                foreach(var infodet in infoloc.Data)
                {
                    if(infodet.Id == info.Idlocation && infodet.Active == true)
                    thereIsLocation = true;
                    break;
                }
                if (!thereIsLocation)
                {
                    return ApiResponseFactory.BadRequest<int>($"This location : {info.Idlocation} not found or there is not active ");

                }

                //WMS inventory type validation

                if (info.Idtype <= 0)
                    return ApiResponseFactory.BadRequest<int>("Inventory type not found");

                
                var infotype = await _commonQueryWMSMasterService.GetAllActiveInventoryTypesByCompanyClient(objcreate.hdr.Idcompany, objcreate.hdr.Codecompanyclient);

                if (infotype.Data.Count <= 0)
                    return ApiResponseFactory.BadRequest<int>($"Active Inventory Types for this company {objcreate.hdr.Idcompany} not found");

                foreach (var infotypeline in infotype.Data)
                {
                    if (infotypeline.Id == info.Idtype)
                        thereIsInventoryType = true;
                    break;
                }
                if (!thereIsInventoryType)
                {
                    return ApiResponseFactory.BadRequest<int>($"This inventory type : {info.Idtype} not found or there is not active ");

                }

                //WMS Rack Validation

                if (info.Idrack <= 0)
                    return ApiResponseFactory.BadRequest<int>("Rack not found");
                
                var inforack = await _commonQueryWMSMasterService.GetAllActiveRacksByCompanyClient(objcreate.hdr.Idcompany, objcreate.hdr.Codecompanyclient);

                if (inforack.Data.Count <= 0)
                    return ApiResponseFactory.BadRequest<int>($"Active Racks Types for this company {objcreate.hdr.Idcompany} not found");

                foreach (var inforackline in inforack.Data)
                {
                    if (inforackline.Id == info.Idrack)
                        thereIsRacks = true;
                    break;
                }
                if (!thereIsRacks)
                {
                    return ApiResponseFactory.BadRequest<int>($"This rack : {info.Idrack} not found or there is not active ");

                }


                if (info.Level < 0)
                    return ApiResponseFactory.BadRequest<int>("Level not found");

                if (info.TotalQty <= 0)
                    return ApiResponseFactory.BadRequest<int>("Quantity must be greater than cero");


                //WMS Item status validation

                if (info.Idstatus <= 0)
                    return ApiResponseFactory.BadRequest<int>("Item status not found");

               
                var infostatus = await _commonQueryWMSMasterService.GetAllActiveItemStatusByCompanyClient(objcreate.hdr.Idcompany, objcreate.hdr.Codecompanyclient);

                if (infostatus.Data.Count <= 0)
                    return ApiResponseFactory.BadRequest<int>($"Active Item Status for this company {objcreate.hdr.Idcompany} not found");

                foreach (var infostatusline in infostatus.Data)
                {
                    if (infostatusline.Id == info.Idstatus)
                        thereIsItemStatus = true;
                    break;
                }
                if (!thereIsItemStatus)
                {
                    return ApiResponseFactory.BadRequest<int>($"This item status : {info.Idstatus} not found or there is not active ");

                }

                if (info.Iditem <= 0)
                    return ApiResponseFactory.BadRequest<int>("Item is required");


                var infoItem = await _masterCompanyclientMasterInventory.GetByIdAsync(info.Iditem);

                if (infoItem.Data == null)
                {
                    return ApiResponseFactory.BadRequest<int>($"Item {info.Iditem} not found");
                }

                if (infoItem.Data.Serialized.ToString() != "1")
                {
                    if (string.IsNullOrEmpty(info.Serialid))
                        return ApiResponseFactory.BadRequest<int>($"Serial is required for this item : {info.Codeitem}");
                }
            }


            var transactionId = await _repository.CreateInventoryTransaction(objcreate);

            return ApiResponseFactory.Ok(transactionId, "OK");
        }
    }
}
