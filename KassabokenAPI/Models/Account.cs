using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace KassabokenAPI.Models
{
    public class Account
    {
        public string? Name { get; set; }
        public double Balance { get; set; }
        public string? Type { get; set; }
    }

    public enum AccountType
    {
        income,
        expense,
        check
    }
}
