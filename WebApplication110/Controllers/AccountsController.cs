using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication110.Data;
using WebApplication110.Models;

namespace WebApplication110.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;
       

        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
           // var detail = await _context.Transaction.Where(t => t.accountNumber == id).ToListAsync();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _context.Account.Where(m => m.CustomerId == userId).ToListAsync();
            if (result != null)
            {
                return View(result);
            }
            else
            {
                return RedirectToAction("Index", "Home");

            }


        }


        public async Task <IActionResult> Myresult()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _context.Account.FirstOrDefaultAsync(m => m.CustomerId == userId);
            if(result != null)
            {
                return View(await _context.Account.ToListAsync());
            }
            else
            {
                return RedirectToAction("Index", "Home");

            }

            
        }


        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.accountNumber == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("accountNumber,AccountTypes,InterestRate,Balance,createdAt,CustomerId")] Account account)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                account.CustomerId = userId;
               
                account.createdAt = DateTime.Now;
                if (account.AccountTypes == Types.Business)
                {
                    account.InterestRate = 0.01;
                }
                else if (account.AccountTypes == Types.Checking)
                {
                    account.InterestRate = 0.02;
                }
                else if (account.AccountTypes == Types.Loan)
                {
                    account.InterestRate = 0.08;
                }
                else if (account.AccountTypes == Types.Term)
                {
                    account.InterestRate = 0.05;
                }

                account.createdAt = DateTime.Now;
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("accountNumber,AccountTypes,InterestRate,Balance,createdAt,CustomerId")] Account account)
        {
            if (id != account.accountNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.accountNumber))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.accountNumber == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Account.FindAsync(id);
            _context.Account.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Account.Any(e => e.accountNumber == id);
        }
    }
}
