using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using StoreKit;
using Foundation;
using UIKit;

namespace RepositoryStumble.Purchases
{
    public class InAppPurchases
    {
        private static readonly Lazy<InAppPurchases> _instance = new Lazy<InAppPurchases>(() => new InAppPurchases());
        private readonly TransactionObserver _observer;
        private TaskCompletionSource<bool> _actionSource;
        private readonly LinkedList<object> _productDataRequests = new LinkedList<object>();

        public static InAppPurchases Instance
        {
            get { return _instance.Value; }
        }

        private void OnPurchaseError(SKPayment id, Exception e)
        {
            if (_actionSource != null)
                _actionSource.TrySetException(e);
        }

        private void OnPurchaseSuccess(SKPayment id)
        {
            if (_actionSource != null)
                _actionSource.TrySetResult(true);
        }

        private InAppPurchases()
        {
            _observer = new TransactionObserver(this);
            SKPaymentQueue.DefaultQueue.AddTransactionObserver(_observer);
        }

        public async Task<SKProductsResponse> RequestProductData (params string[] productIds)
        {
            var array = new NSString[productIds.Length];
            for (var i = 0; i < productIds.Length; i++)
                array[i] = new NSString(productIds[i]);

            var tcs = new TaskCompletionSource<SKProductsResponse>();
            _productDataRequests.AddLast(tcs);

            try
            {
                var productIdentifiers = NSSet.MakeNSObjectSet<NSString>(array); //NSSet.MakeNSObjectSet<NSString>(array);​​​
                var productsRequest = new SKProductsRequest(productIdentifiers);
                productsRequest.ReceivedResponse += (sender, e) => tcs.SetResult(e.Response);
                productsRequest.RequestFailed += (sender, e) => tcs.SetException(new Exception(e.Error.LocalizedDescription));
                productsRequest.Start();
                var ret = await tcs.Task;
                productsRequest.Dispose();
                return ret;
            }
            finally
            {
                _productDataRequests.Remove(tcs);
                Console.WriteLine("Remaining: " + _productDataRequests.Count);
            }
        }

        public static bool CanMakePayments()
        {
            return SKPaymentQueue.CanMakePayments;        
        }

        public Task Restore()
        {
            _actionSource = new TaskCompletionSource<bool>();
            SKPaymentQueue.DefaultQueue.RestoreCompletedTransactions();
            return _actionSource.Task;
        }

        public async Task PurchaseProduct(SKProduct productId)
        {
            _actionSource = new TaskCompletionSource<bool>();
            SKPayment payment = SKPayment.PaymentWithProduct(productId);
            SKPaymentQueue.DefaultQueue.AddPayment (payment);
            await _actionSource.Task;
        }

        private void CompleteTransaction (SKPaymentTransaction transaction)
        {
            Console.WriteLine ("CompleteTransaction " + transaction.TransactionIdentifier);
            OnPurchaseSuccess(transaction.Payment);
        }

        private void RestoreTransaction (SKPaymentTransaction transaction)
        {
            Console.WriteLine("RestoreTransaction " + transaction.TransactionIdentifier + "; OriginalTransaction " + transaction.OriginalTransaction.TransactionIdentifier);
            OnPurchaseSuccess(transaction.OriginalTransaction.Payment);
        }

        private void FailedTransaction (SKPaymentTransaction transaction)
        {
            var errorString = transaction.Error != null ? transaction.Error.LocalizedDescription : "Unable to process transaction!";
            OnPurchaseError(transaction.Payment, new Exception(errorString));
        }

        private class TransactionObserver : SKPaymentTransactionObserver
        {
            private readonly InAppPurchases _inAppPurchases;
            private UIAlertView _alertView;

            public TransactionObserver(InAppPurchases inAppPurchases)
            {
                _inAppPurchases = inAppPurchases;
            }

            public override void UpdatedTransactions(SKPaymentQueue queue, SKPaymentTransaction[] transactions)
            {
                foreach (SKPaymentTransaction transaction in transactions)
                {
                    try
                    {

                        switch (transaction.TransactionState)
                        {
                            case SKPaymentTransactionState.Purchased:
                                _inAppPurchases.CompleteTransaction(transaction);
                                SKPaymentQueue.DefaultQueue.FinishTransaction(transaction);
                                break;
                            case SKPaymentTransactionState.Failed:
                                _inAppPurchases.FailedTransaction(transaction);
                                SKPaymentQueue.DefaultQueue.FinishTransaction(transaction);
                                break;
                            case SKPaymentTransactionState.Restored:
                                _inAppPurchases.RestoreTransaction(transaction);
                                SKPaymentQueue.DefaultQueue.FinishTransaction(transaction);
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        var alert = _alertView = new UIAlertView();
                        alert.Title = "Error";
                        alert.Message = "Unable to process transaction: " + e.Message;
                        alert.Show();
                    }
                }
            }

            //            public override void RemovedTransactions(SKPaymentQueue queue, SKPaymentTransaction[] transactions)
            //            {
            //                foreach (var t in transactions)
            //                {
            //                    Console.WriteLine("Uh oh: " + t.TransactionState);
            //
            //                }
            //            }

            public override void PaymentQueueRestoreCompletedTransactionsFinished (SKPaymentQueue queue)
            {
                Console.WriteLine(" ** RESTORE PaymentQueueRestoreCompletedTransactionsFinished ");
                if (_inAppPurchases._actionSource != null)
                    _inAppPurchases._actionSource.TrySetResult(true);
            }

            public override void RestoreCompletedTransactionsFailedWithError (SKPaymentQueue queue, NSError error)
            {
                Console.WriteLine(" ** RESTORE RestoreCompletedTransactionsFailedWithError " + error.LocalizedDescription);
                if (_inAppPurchases._actionSource != null)
                    _inAppPurchases._actionSource.TrySetResult(true);
            }
        }
    }

    public static class SKProductExtension 
    {
        public static string LocalizedPrice (this SKProduct product)
        {
            var formatter = new NSNumberFormatter ();
            formatter.FormatterBehavior = NSNumberFormatterBehavior.Version_10_4;  
            formatter.NumberStyle = NSNumberFormatterStyle.Currency;
            formatter.Locale = product.PriceLocale;
            var formattedString = formatter.StringFromNumber(product.Price);
            return formattedString;
        }
    }
}
