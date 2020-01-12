using LibraryData2;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Linq;
using Library.Models.Catalog;
using Library.Models;
using Library.Models.Checkout;

namespace Library.Controllers
{
    public class CatalogController:Controller
    {
        private ILibraryAsset _assets;
        private ICheckout _checkout;
        public CatalogController(ILibraryAsset asset,ICheckout checkout)
        {
            _assets = asset;
            _checkout = checkout;
        }
        public IActionResult Index()
        {
            var assetModels = _assets.GetAll();

            var listingResult = assetModels.Select(result => new AssetIndexListingModel
            {
                id = result.Id,
                ImgUrl = result.ImageUrl,
                Title = result.Title,
                AuthorOrDirector=_assets.GetAuthorOrDirector(result.Id),
                DewyCallNumber=_assets.GetDewyIndex(result.Id),
                Type=_assets.GetType(result.Id),


            });

            var model = new AssetIndexModel()
            {
                assets=listingResult
            };

            return View(model);
        }
        public IActionResult Detail(int id)
        {
            var asset = _assets.GetAsset(id);

            var currentHolds = _checkout.GetCurrentHolds(id).Select(a => new AssetHoldModel
            {
                HoldPlaced = _checkout.GetCurrentHoldPlaced(a.Id).ToString("d"),
                PatronName = _checkout.GetCurrentHoldPatronName(a.Id)
            });

            var model = new AssetDetailModel
            {
                AssetId = id,
                Title = asset.Title,
                Type = _assets.GetType(id),
                Year = asset.Year,
                Cost = asset.Cost,
                Isbn = _assets.GetIsbn(id),
                Status = asset.Status.Name,
                ImageUrl = asset.ImageUrl,
                AuthorOrDirector = _assets.GetAuthorOrDirector(id),
                CurrentLocation = _assets.GetCurrentLocation(id)?.Name,
                Dewey = _assets.GetDewyIndex(id),
                CheckoutHistory = _checkout.GetCheckoutHistories(id),
                //   CurrentAssociatedLibraryCard = _ass,
                //  Isbn = _assets.GetIsbn(id),
                latestCheckout = _checkout.GetLatestCheckout(id),
                CurrentHolds = currentHolds,
                PatronName = _checkout.GetCurrentCheckoutPatron(id)
            };

            return View(model);
        }

        public IActionResult Checkout(int id)
        {
            var asset = _assets.GetAsset(id);
            var model = new CheckoutModels
            {
                AssetId=id,
                ImageUrl=asset.ImageUrl,
                Title=asset.Title,
                LibraryCardId="",
                IsCheckedOut=_checkout.IsCheckedOut(id)
            };
            return View(model);

        }
        public IActionResult CheckIn(int id,int libraryCardId)
        {
            _checkout.CheckInItem(id, libraryCardId);
            return RedirectToAction("Detail", new { id = id });
        }
        public IActionResult Hold(int id)
        {
            var asset = _assets.GetAsset(id);
            var model = new CheckoutModels
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                IsCheckedOut = _checkout.IsCheckedOut(id),
                HoldCount = _checkout.GetCurrentHolds(id).Count()
                
            };
            return View(model);

        }
        public IActionResult MarkLost(int id)
        {
            _checkout.MarkLost(id);
            return RedirectToAction("Detail", new { id = id });
        }
        public IActionResult MarkFound(int id)
        {
            _checkout.MarkFound(id);
            return RedirectToAction("Detail", new { id = id });
        }
        [HttpPost]
        public IActionResult PlaceCheckOut(int assetId,int libraryCardId)
        {
            _checkout.CheckOutItem(assetId,libraryCardId);
            return RedirectToAction("Detail", new { id = assetId });
        }
        [HttpPost]
        public IActionResult PlaceHold(int assetId, int libraryCardId)
        {
            _checkout.PlaceHold(assetId, libraryCardId);
            return RedirectToAction("Detail", new { id = assetId });
        }
    }
}
