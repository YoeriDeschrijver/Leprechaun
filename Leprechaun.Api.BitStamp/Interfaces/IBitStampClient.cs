using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leprechaun.Api.BitStamp
{
    /// <summary>
    /// BitStampClient interface
    /// </summary>
    public interface IBitStampClient
    {
        #region NO AUTHORIZATION
        /// <summary>
        /// Get the lastest rate info.
        /// </summary>
        /// <returns></returns>
        RateInfo GetRateInfo();

        /// <summary>
        /// Get the order book
        /// </summary>
        /// <returns></returns>
        OrderBook GetOrderBook();

        /// <summary>
        /// Get the transactions
        /// </summary>
        /// <returns></returns>
        List<Transaction> GetTransactions();
        #endregion

        #region AUTHORIZATION REQUIRED
        /// <summary>
        /// Get users BitCoin deposit address.
        /// </summary>
        /// <param name="signature"></param>
        /// <returns>deposit address</returns>
        string GetBitCoinDepositAddress(BitStampSignature signature);

        /// <summary>
        /// Get balance.
        /// </summary>
        /// <returns></returns>
        Balance GetBalance(BitStampSignature signature);

        /// <summary>
        /// Get users transactions.
        /// </summary>
        /// <returns></returns>
        List<UserTransaction> GetUserTransactions(BitStampSignature signature, int offset = 0, int limit = 100, string sort = "desc");

        /// <summary>
        /// Get users open orders.
        /// </summary>
        /// <returns></returns>
        List<Order> GetOpenOrders(BitStampSignature signature);

        /// <summary>
        /// Cancel order
        /// </summary>
        /// <returns></returns>
        bool CancelOrder(BitStampSignature signature, string orderID);

        /// <summary>
        /// Buy Bitcoins. This uses limited orders.
        /// </summary>
        /// <param name="signature">Signature</param>
        /// <param name="amount">Amount</param>
        /// <param name="price">Price in USD</param>
        /// <returns>Order</returns>
        Order Buy(BitStampSignature signature, decimal amount, decimal? price = null);

        /// <summary>
        /// Sell Bitcoins. This uses limited orders.
        /// </summary>
        /// <param name="signature">Signature</param>
        /// <param name="amount">Amount</param>
        /// <param name="price">Price in USD</param>
        /// <returns>Order</returns>
        Order Sell(BitStampSignature signature, decimal amount, decimal? price = null);
        #endregion 
    }
}
