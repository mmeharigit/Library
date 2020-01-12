using LibraryData2.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData2
{
   public interface ICheckout
    {
        IEnumerable<Checkout> GetAll();
        Checkout GetById(int checkoutId);
        Checkout GetLatestCheckout(int assetId);
        void Add(Checkout newCheckout);
        void CheckOutItem(int assetId, int libraryCardId);
        void CheckInItem(int assetId, int libraryCardId);
        void MarkLost(int assetId);
        void MarkFound(int assetId);
        bool IsCheckedOut(int assetId);
        IEnumerable<CheckoutHistory> GetCheckoutHistories(int id);
        void PlaceHold(int assetId, int LibraryCard);
        string GetCurrentHoldPatronName(int id);
        string GetCurrentCheckoutPatron(int assetId);
        DateTime GetCurrentHoldPlaced(int id);
        IEnumerable<Hold> GetCurrentHolds(int id);
        



    }
}
