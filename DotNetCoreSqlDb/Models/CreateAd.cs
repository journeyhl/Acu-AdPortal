using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Web;
using System.Data.SqlTypes;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DotNetCoreSqlDb.Models
{
    [BindProperties]
    public class CreateAd
    {


        [Required(ErrorMessage = "The Ad Start Date field is required")] public DateTime? Date { get; set; }

        [Required(ErrorMessage = "The Spend Date field is required")] public DateTime? SpendDate { get; set; }
        
        public List<string> AdCategory { get; set; }
        public CreateAd()
        {
            AdCategory = new List<string>()
            {
                {"TV"},{"MEDIA"}, {"WEB"}
            };
        }

        /*PrimaryAd*/
        [Required] public string myInput  { get; set; }
        public string mySecondary { get; set; }


        [RegularExpression(@"/\s|[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]/g", ErrorMessage = "This field only accepts Number or Letter values")] public string AdVersionID { get; set; }


        public string AdVersionProduct { get; set; }

        public string Region { get; set; }

        public int CheckPhone { get; set; }


        [RegularExpression(@"^[0-9]+$", ErrorMessage = "This field only accepts Number values")] public string? Circulation { get; set; }

        [RegularExpression(@"^(0|[1-9]\d*)(\.\d+)?$", ErrorMessage = "This field only accepts Number values")][Required] public string AdSpendAmount { get; set; }


        [RegularExpression(@"^[0-9]+$", ErrorMessage = "This field only accepts Number values")] public string? PageNumber { get; set; }


        public string AdCodeInternal { get; set; }

        public string AdCodeExternal { get; set; }

        public string CustomAdCode { get; set; }

        [StringLength(13, MinimumLength = 10)]public string TFN { get; set; }

        public string PhoneAdCode { get; set; }


    }








    [BindProperties]
    public class Internal
    {
        public string AdCode { get; set; }
    }
    [BindProperties]
    public class External
    {
       public string AdCode { get; set; }
    }

    

}
