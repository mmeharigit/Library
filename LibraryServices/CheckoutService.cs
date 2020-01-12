using LibraryData2;
using LibraryData2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryServices
{
    public class CheckoutService : ICheckout
    {
        //Dependency Injection
        private LibraryContext _context;
        public CheckoutService(LibraryContext context)
        {
            _context = context;
        }
        public void Add(Checkout newCheckout)
        {
            _context.Add(newCheckout);
            _context.SaveChanges();
        }

        public void CheckInItem(int assetId, int libraryCardId)
        {
            var now = DateTime.Now;
            var item = _context.libraryAssets.First(a => a.Id == assetId);
           // _context.Update(item);
            //remove any existing checkouts on the item.
            RemoveExistingCheckouts(assetId);
            //close any existing checkout history
            CloseExistingCheckOutHistory(assetId, now);
            //look for existing holds on the item
            var currentHolds = _context.Holds
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .Where(h => h.LibraryAsset.Id == assetId);

            //if there are holds,checkout the item to the libraryCard with the earlest hold
            if(currentHolds.Any())
            {
                CheckoutToEarliestHold(assetId, currentHolds);
               return;
            }
            //otherwise, update the item status to avaliable.
            
          
                UpdateAssetStatus(assetId, "Available");
            _context.SaveChanges();
            
          //  UpdateAssetStatus(assetId, "Available"); 
        }

        private void CheckoutToEarliestHold(int assetId, IQueryable<Hold> currentHolds)
        {
            var earliestHold = currentHolds
                .OrderBy(holds => holds.HoldPlaced)
                .FirstOrDefault();
            var card = earliestHold.LibraryCard;
            _context.Remove(earliestHold);
            _context.SaveChanges();
            CheckOutItem(assetId, card.Id);

        }

        public void CheckOutItem(int assetId, int libraryCardId)
        {
            if(IsCheckedOut(assetId))
            {
                return;
                //Add Logic here to handle feedback to the user
            }

            var item = _context.libraryAssets.Include(s=>s.Status)
                .First(a => a.Id == assetId);
          //  _context.Update(item);// mark it for later save.
            UpdateAssetStatus(assetId, "Checked Out");

            var libraryCard = _context
                .LibraryCards
                .Include(card => card.Checkouts)
                .FirstOrDefault(card => card.Id == libraryCardId);

            var now = DateTime.Now;
            var checkout = new Checkout
            {
                LibraryAsset = item,
                LibraryCard = libraryCard,
                Since = now,
                Until = GetDefaultCheckOutTime(now)

            };
            _context.Update(checkout);
            _context.Add(checkout);

            var checkouthistory = new CheckoutHistory
            {
                CheckedOut = now,
                LibraryAsset = item,
                libraryCard = libraryCard
            };
            //_context.Update(checkouthistory);
            _context.Add(checkouthistory);
            _context.SaveChanges();
        }

        private DateTime GetDefaultCheckOutTime(DateTime now)
        {
            return now.AddDays(30);
        }

        public bool IsCheckedOut(int assetId)
        {
            var isCheckedOut = _context
                .Checkouts
                .Where(co => co.LibraryAsset.Id == assetId)
                .Any();
            return isCheckedOut;
        }

        public IEnumerable<Checkout> GetAll()
        {
            return _context.Checkouts;
        }

        public Checkout GetById(int checkoutId)
        {
            return GetAll()
                .FirstOrDefault(checkout => checkout.Id == checkoutId);
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistories(int id)
        {
            return _context.CheckoutHistories
                .Include(h=>h.LibraryAsset)
                .Include(h=>h.libraryCard)
                .Where(h => h.LibraryAsset.Id == id);
        }

        public string GetCurrentHoldPatronName(int id)
        {
            var hold = _context
                .Holds
                .Include(ho => ho.LibraryAsset)
                .Include(ho => ho.LibraryCard)
                .FirstOrDefault(ho => ho.Id == id);
            var card = hold?.LibraryCard.Id;
            var patron = _context
                .Patrons
                .Include(p => p.LibraryCard)
                .FirstOrDefault(p => p.LibraryCard.Id == card);
            return patron?.FirstName + " " + patron?.LastName;
        }

        public DateTime GetCurrentHoldPlaced(int id)
        {
            var hold = _context
                .Holds
                .Include(ho => ho.LibraryAsset)
                
                .Include(ho => ho.LibraryCard)
                .FirstOrDefault(ho => ho.Id == id).HoldPlaced;
            return hold;
        }

        public IEnumerable<Hold> GetCurrentHolds(int id)
        {
            return _context.Holds
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .Where(assetId => assetId.LibraryAsset.Id == id);

        }

        public Checkout GetLatestCheckout(int assetId)
        {
            var latestCheckout = _context.Checkouts
                .Where(checkout => checkout.LibraryAsset.Id == assetId)
                .OrderByDescending(c => c.Since)
                .FirstOrDefault();
            return latestCheckout;
        }

        public void MarkFound(int assetId)
        {
            var now = DateTime.Now;
            var item = _context.libraryAssets.FirstOrDefault(asset => asset.Id == assetId);
            //mark it for update
           // _context.Update(item);
            //change the item status to Avaliable

         //   item.Status = _context.Statuses.FirstOrDefault(status => status.Name == "Available");
            UpdateAssetStatus(assetId, "Available");
            //remove any existing checkout on the current item
            RemoveExistingCheckouts(assetId);

            //close any checkedOut item as CheckedIn
            CloseExistingCheckOutHistory(assetId,now);
            _context.SaveChanges();
        }

        private void UpdateAssetStatus(int assetId, string v)
        {
            var item = _context.libraryAssets.Include(ch=>ch.Status).First(asset => asset.Id == assetId);
            //mark it for update
            _context.Update(item);
            //change the item status to Avaliable

            item.Status = _context.Statuses.FirstOrDefault(status => status.Name == v);
        }

        private void CloseExistingCheckOutHistory(int assetId,DateTime now)
        {
            var checkoutHistory = _context
                .CheckoutHistories
                .FirstOrDefault(ch => ch.LibraryAsset.Id == assetId && ch.CheckedIn == null);
            if(checkoutHistory != null)
            {
                _context.Update(checkoutHistory);
                checkoutHistory.CheckedIn = now;

            }
        }

        private void RemoveExistingCheckouts(int assetId)
        {
            var checkout = _context.Checkouts.FirstOrDefault(co => co.LibraryAsset.Id == assetId);
            if(checkout!=null)
            {
                _context.Remove(checkout);
            }
        }

        public void MarkLost(int assetId)
        {
           
            UpdateAssetStatus(assetId, "Lost");
            _context.SaveChanges();

        }

        public void PlaceHold(int assetId, int LibraryCard)
        {
            var now = DateTime.Now;
            var asset = _context.libraryAssets.Include(s=>s.Status).FirstOrDefault(c => c.Id == assetId);
            var card = _context.LibraryCards.FirstOrDefault(c => c.Id == LibraryCard);
            if(asset.Status.Name=="Available")
            {
                UpdateAssetStatus(assetId, "On Hold");
            }
            var hold = new Hold
            {
                HoldPlaced = now,
                LibraryAsset = asset,
                LibraryCard = card
            };
            _context.Add(hold);
            _context.SaveChanges();
        }

        public string GetCurrentCheckoutPatron(int assetId)
        {
            var checkout = GetCheckoutByAssetId(assetId);
            if (checkout == null)
            {
                return "";
            }
            //   var card=_context.Checkouts.Include(co=>co.LibraryAsset).Include(co=>co.LibraryCard).FirstOrDefault(card=>card.LibraryCard.Id==assetid)
            var cardId = checkout.LibraryCard.Id;
            var patron = _context.Patrons
                .Include(p => p.LibraryCard)
                .FirstOrDefault(p => p.LibraryCard.Id == cardId);
            return patron.FirstName + " " + patron.LastName;
        }

        private Checkout GetCheckoutByAssetId(int assetId)
        {
            return _context
                .Checkouts
                .Include(co => co.LibraryAsset)
                .Include(co => co.LibraryCard)
                .FirstOrDefault(co => co.LibraryAsset.Id == assetId);
        }
    }
}
