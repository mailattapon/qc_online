using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QcSupplier.Entities;
using QcSupplier.Models;
using QcSupplier.Repositories;

namespace QcSupplier.Infrastructures {
  public class AppUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Role> {
    #region Field

    private readonly ProgramRepository _programRepo;
    private readonly UserRepository _userRepo;

    #endregion

    #region Constructor

    public AppUserClaimsPrincipalFactory(
      UserManager<User> userManager,
      RoleManager<Role> roleManager,
      IOptions<IdentityOptions> optionsAccessor,
      ProgramRepository programRepo,
      UserRepository userRepo
    ) : base(userManager, roleManager, optionsAccessor) {
      _programRepo = programRepo;
      _userRepo = userRepo;
    }

    #endregion

    #region Method

    protected async override Task<ClaimsIdentity> GenerateClaimsAsync(User user) {
      var identity = await base.GenerateClaimsAsync(user);
      var programs = await _programRepo.FindListAsync();
      var uPrograms = await _userRepo.FindByIdAsync(user.Id);
      var claims = new List<ProgramClaim>();
      foreach (var p in programs) {
        var enabled = uPrograms.UserPrograms.Any(up => up.ProgramId == p.Id);
        if (enabled) {
          identity.AddClaim(new Claim(p.PolicyName, string.Empty));
        }
        var c = new ProgramClaim {
          Id = p.Id,
          Name = p.Name,
          Enabled = enabled,
          ControllerName = p.PolicyName
        };
        claims.Add(c);
      }
      identity.AddClaim(new Claim("Programs", JsonConvert.SerializeObject(claims), JsonClaimValueTypes.Json));
      return identity;
    }

    #endregion

  }
}