using LibraryData2;
using LibraryData2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryServices
{
    public class LibraryAssetService : ILibraryAsset

    {
        private LibraryContext _context;
        public LibraryAssetService(LibraryContext context)
        {
            _context = context;
        }
        public void Add(LibraryAsset newAsset)
        {
            _context.Add(newAsset);
            _context.SaveChanges();
        }

        public IEnumerable<LibraryAsset> GetAll()
        {
            return _context.libraryAssets
                .Include(asset => asset.Status)
                .Include(asset => asset.Location);
        }

        public LibraryAsset GetAsset(int id)
        {
            return _context.libraryAssets
                 .Include(asset => asset.Status)
                 .Include(asset => asset.Location).FirstOrDefault(asset => asset.Id == id);
                
        }

        public string GetAuthorOrDirector(int id)
        {
            var isBook = _context.libraryAssets.OfType<Book>().Where(asset => asset.Id == id).Any();
            var isVideo = _context.libraryAssets.OfType<Video>().Where(asset => asset.Id == id).Any();
            return isBook ? _context.Books.FirstOrDefault(book => book.Id == id).Author :
                _context.Videos.FirstOrDefault(video => video.Id == id).Director
                ?? "Unkown";
        }

        public LibraryBranch GetCurrentLocation(int id)
        {
            return _context
                .libraryAssets
                .FirstOrDefault(asset => asset.Id == id).Location;
        }

        public string GetDewyIndex(int id)
        {
            //we have Discriminator to separate Video and Book Assets
            if (_context.Books.Any(asset => asset.Id == id))
            {
                return _context.Books.FirstOrDefault(book => book.Id == id).DewyIndexNumber;
            }
            else
                return "";
        }

        public string GetIsbn(int id)
        {
            if (_context.Books.Any(book => book.Id == id))
            {
                return _context.Books.FirstOrDefault(book => book.Id == id).ISBN;
            }
            else return "";
        }

        public string GetTitle(int id)
        {
            return _context.libraryAssets.FirstOrDefault(asset => asset.Id == id).Title;
        }

        public string GetType(int id)
        {
            var isBook = _context.libraryAssets.OfType<Book>().Where(asset => asset.Id == id);

            return isBook.Any() ? "Book" : "Video";
        }
    }
}
