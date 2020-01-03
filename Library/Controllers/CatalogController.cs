using LibraryData2;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Linq;
using Library.Models.Catalog;
using Library.Models;

namespace Library.Controllers
{
    public class CatalogController:Controller
    {
        private ILibraryAsset _assets;
        public CatalogController(ILibraryAsset asset)
        {
            _assets = asset;
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

            //var currentHolds = _assets.GetCurrentHolds(id).Select(a => new AssetHoldModel
            //{
            //    HoldPlaced = _assets.GetCurrentHoldPlaced(a.Id),
            //    PatronName = _assets.GetCurrentHoldPatron(a.Id)
            //});

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
                Dewey = _assets.GetDewyIndex(id)
                //CheckoutHistory = _checkoutsService.GetCheckoutHistory(id),
                //CurrentAssociatedLibraryCard = _assetsService.GetLibraryCardByAssetId(id),
                //Isbn = _assets.GetIsbn(id),
                //LatestCheckout = _checkoutsService.GetLatestCheckout(id),
                //CurrentHolds = currentHolds,
                //PatronName = _checkoutsService.GetCurrentPatron(id)
            };

            return View(model);
        }
    }
}
