using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BooksLibrary.Models
{
    public static class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
             new Claim("Create Claim" , "Create Claim"),
             new Claim("Edit Claim" , "Edit Claim"),
             new Claim("Delete Claim" , "Delete Claim")
    };
        
    }
}
