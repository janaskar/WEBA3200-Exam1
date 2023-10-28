#nullable disable

using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using PowerCards.Models;

namespace  PowerCards.Areas.Identity.Pages.Account.Manage
{
    public static class ManageNavPages
    {
        public static string MyDecks => "MyDecks";
        public static string Favourites => "Favourites";
        public static string UserSettings => "UserSettings";
        public static string MyDecksNavClass(ViewContext viewContext) => PageNavClass(viewContext, MyDecks);
        public static string FavouritesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Favourites);
        public static string UserSettingsNavClass(ViewContext viewContext) => PageNavClass(viewContext, UserSettings);
        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}