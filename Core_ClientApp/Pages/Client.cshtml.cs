using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using Core_ClientApp.Services;
namespace Core_ClientApp.Pages
{
    [AuthorizeForScopes(Scopes = new string[] { "api://929434f4-8a56-4b8b-b7c5-c8b16a960661/access_api_user" })]
    public class ClientModel : PageModel
    {
        private readonly ServiceClient service;

        public string ResponseData { get; set; }
        public ClientModel(ServiceClient serv)
        {
            service = serv;
        }

        public async Task OnGetAsync()
        {
            ResponseData = await service.GetDataAsync();
        }
    }
}
