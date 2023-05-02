using KassabokenAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KassabokenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        static List<Account> accounts = new List<Account>();
        //{
        //    new Account { Name = "Lön", Type = AccountType.income.ToString(), Balance = 0 },
        //    new Account { Name = "Hyra", Type = AccountType.expense.ToString(), Balance = 0 },
        //    new Account { Name = "Livsmedel", Type = AccountType.expense.ToString(), Balance = 0 },
        //    new Account { Name = "Bankkonto", Type = AccountType.check.ToString(), Balance = 0 }
        //};
        static List<Transaction> transactions = new List<Transaction>();

        [HttpPost("account")]
        public IActionResult CreateAccount(Account account)
        {
            if (Enum.TryParse(account.Type, out AccountType accountType))
            {
                if (account.Name?.Length > 20)
                {
                    account.Name = account.Name.Substring(0, 20);
                }
                accounts.Add(account);
                return CreatedAtAction(nameof(CreateAccount), new { name = account.Name, account.Type }, account);
            }
            return BadRequest();
        }

        [HttpGet("account")]
        public IEnumerable<object> GetAllAccounts()
        {
            return accounts
                .OrderBy(x => x.Name)
                .Select(x => new { name = x.Name, balance = x.Balance });
        }

        [HttpPost("transaction")]
        public IActionResult MakeTransaction(Transaction transaction)
        {
            if (transaction.Amount <= 0 || transaction.Amount > 50000)
            {
                return BadRequest();
            }
            var fromAccount = accounts
                .SingleOrDefault(p => p.Name == transaction.FromAccount)!;

            var toAccount = accounts
                .SingleOrDefault(p => p.Name == transaction.ToAccount)!;

            if (fromAccount.Type == AccountType.check.ToString())
            {
                fromAccount.Balance -= transaction.Amount;
            }
            else if (fromAccount.Type == AccountType.income.ToString() || fromAccount.Type == AccountType.expense.ToString())
            {
                fromAccount.Balance += transaction.Amount;
            }

            if (toAccount.Type == AccountType.income.ToString() || toAccount.Type == AccountType.expense.ToString())
            {
                toAccount.Balance -= transaction.Amount;
            }
            else if (toAccount.Type == AccountType.check.ToString())
            {
                toAccount.Balance += transaction.Amount;
            }

            transactions.Add(transaction);
            return CreatedAtAction(nameof(MakeTransaction), new { fromAccount = transaction.FromAccount, toAccount = transaction.ToAccount, amount = transaction.Amount }, transaction);
        }

        [HttpGet("transaction")]
        public IEnumerable<Transaction> GetAllTranscation()
        {
            return transactions;
        }
    }
}
