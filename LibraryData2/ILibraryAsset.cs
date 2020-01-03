using LibraryData2.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData2
{
   public interface ILibraryAsset
    {
        IEnumerable<LibraryAsset> GetAll();
        LibraryAsset GetAsset(int id);
        void Add(LibraryAsset newAsset);
        string GetAuthorOrDirector(int id);
        string GetDewyIndex(int id);
        string GetType(int id);
        string GetTitle(int id);
        string GetIsbn(int id);
        LibraryBranch GetCurrentLocation(int id);
    }
}
