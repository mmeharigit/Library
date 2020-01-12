using LibraryData2.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData2
{
    public interface IPatronService
    {
        IEnumerable<Patron> GetAll();
        Patron Get(int id);
        void Add(Patron newBook);
        IEnumerable<CheckoutHistory> GetCheckoutHistory(int patronId);
        IEnumerable<Hold> GetHolds(int patronId);
        IEnumerable<Checkout> GetCheckouts(int id);
    }
}
