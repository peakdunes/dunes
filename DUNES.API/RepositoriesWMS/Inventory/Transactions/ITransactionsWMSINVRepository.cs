﻿using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.TemporalModels;

namespace DUNES.API.RepositoriesWMS.Inventory.Transactions
{

    /// <summary>
    /// all WMS inventory transactions
    /// </summary>
    public interface ITransactionsWMSINVRepository
    {

        /// <summary>
        /// Create inventory transaction
        /// </summary>
        /// <param name="objcreate"></param>
        /// <returns></returns>
        Task<int> CreateInventoryTransaction(NewInventoryTransactionTm objcreate);
    }
}
