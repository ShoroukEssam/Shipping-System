using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shipping.DTO.AccountDTOs;
using Shipping.DTO.MerchantDTOs;
using Shipping.Models;
using Shipping.Repository.MerchantRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class MerchantRepository : IMerchantRepository
{
    private readonly ShippingContext _context;
    private readonly UserManager<AppUser> _userManager;

    public MerchantRepository(ShippingContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<List<Merchant>> GetAllMerchants()
    {
        return await _context.Merchants.Where(m=>m.User.IsDeleted==false)     
            .ToListAsync();
    }

    public async Task<Merchant> Add(MerchantDTO newMerchant, UserManager<AppUser> _userManager)
    {
        // Check if email already exists
        if (await _userManager.FindByEmailAsync(newMerchant.email) != null)
        {
            throw new Exception("Email already exists.");
        }

        var user = new AppUser
        {
            UserName = newMerchant.email,
            Email = newMerchant.email,
            PhoneNumber = newMerchant.phone,
            Name = newMerchant.name,
            Status = newMerchant.status ?? true
        };

        var result = await _userManager.CreateAsync(user, newMerchant.password);
        if (!result.Succeeded)
        {
            throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }
        await _userManager.AddToRoleAsync(user, "التجار");
        var branch = await _context.Branches.FirstOrDefaultAsync(b => b.Name == newMerchant.branchName);
        if (branch == null)
        {
            throw new Exception("Branch not found.");
        }

        var city = await _context.Cities.FirstOrDefaultAsync(c => c.Name == newMerchant.city);
        if (city == null)
        {
            throw new Exception("City not found.");
        }

        var government = await _context.Governments.FirstOrDefaultAsync(g => g.Name == newMerchant.government);
        if (government == null)
        {
            throw new Exception("Government not found.");
        }

        var merchant = new Merchant
        {
            UserId = user.Id,
            BranchId = branch.Id,
            PickUpSpecialCost = newMerchant.pickUpSpecialCost,
            RefusedOrderPercent = newMerchant.refusedOrderPercent,
            CityId = city.Id,
            GovernmentId = government.Id,
            SpecialCitiesPrices=newMerchant.SpecialCitiesPrices.Select
            (op => new SpecialCitiesPrice
            {
                Government=op.Government,
                City=op.City,
                Price=op.Price
            }).ToList()
            

        };
        _context.Merchants.Add(merchant);
        await _context.SaveChangesAsync();

        return merchant;
    }

    public async Task<Merchant> GetMerchantByIdAsync(string id)
    {
        return await _context.Merchants        
            .FirstOrDefaultAsync(m => m.UserId == id);
    }

    public List<Merchant> Search(string query)
    {
        return _context.Merchants
            
            .Where(m => m.User.Name.Contains(query) ||
                        m.User.Email.Contains(query) ||
                        m.Branch.Name.Contains(query) ||
                        m.City.Name.Contains(query) ||
                        m.Government.Name.Contains(query))
            .ToList();
    }

    public async Task<Merchant> Update(MerchantDTO newData, UserManager<AppUser> _userManager)
    {
        var merchant = await GetMerchantByIdAsync(newData.id);
        if (merchant == null)
        {
            throw new Exception("Merchant not found.");
        }

        // Update merchant details
        merchant.User.Name = newData.name;
        merchant.User.Email = newData.email;
        merchant.User.PhoneNumber = newData.phone;

        var branch = await _context.Branches.FirstOrDefaultAsync(b => b.Name == newData.branchName);
        if (branch == null)
        {
            throw new Exception("Branch not found.");
        }
        merchant.BranchId = branch.Id;

        var city = await _context.Cities.FirstOrDefaultAsync(c => c.Name == newData.city);
        if (city == null)
        {
            throw new Exception("City not found.");
        }
        merchant.CityId = city.Id;

        var government = await _context.Governments.FirstOrDefaultAsync(g => g.Name == newData.government);
        if (government == null)
        {
            throw new Exception("Government not found.");
        }
        merchant.GovernmentId = government.Id;

        merchant.PickUpSpecialCost = newData.pickUpSpecialCost;
        merchant.RefusedOrderPercent = newData.refusedOrderPercent;
        if (merchant.SpecialCitiesPrices == null)
        {
            merchant.SpecialCitiesPrices = new List<SpecialCitiesPrice>();
        }

        merchant.SpecialCitiesPrices.Clear();
        merchant.SpecialCitiesPrices.AddRange(newData.SpecialCitiesPrices.Select(op => new SpecialCitiesPrice
        {
            Government = op.Government,
            City = op.City,
            Price = op.Price
        }));

        _context.Merchants.Update(merchant);
        await _context.SaveChangesAsync();

        return merchant;
    }

    public async Task<Merchant> UpdateStatus(Merchant merchant, bool status)
    {
        merchant.User.Status = status;
        _context.Merchants.Update(merchant);
        await _context.SaveChangesAsync();
        return merchant;
    }

    public async Task SoftDeleteAsync(Merchant merchant)
    {
        merchant.User.Status = false;
        merchant.User.IsDeleted = true;
        _context.Merchants.Update(merchant);
        await _context.SaveChangesAsync();
    }
}
