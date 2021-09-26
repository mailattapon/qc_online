using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QcSupplier.Constants;
using QcSupplier.Entities;
using QcSupplier.Extensions;
using QcSupplier.Models;
using QcSupplier.Repositories;
using QcSupplier.ViewModels;

namespace QcSupplier.Controllers {
  [Route("[Controller]")]
  [Authorize(Roles = Roles.SUPER_ADMIN)]
  public class UserController : Controller {
    #region Field

    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly DepartmentRepository _deptRepo;
    private readonly ProgramRepository _programRepo;
    private readonly RoleManager<Role> _roleManager;
    private readonly UserRepository _userRepo;
    private readonly VendorRepository _vendorRepo;

    #endregion

    #region Constructor

    public UserController(
      IMapper mapper,
      UserManager<User> userManager,
      RoleManager<Role> roleManager,
      UserRepository userRepo,
      VendorRepository vendorRepo,
      ProgramRepository programRepo,
      DepartmentRepository deptRepo
    ) {
      _mapper = mapper;
      _userManager = userManager;
      _roleManager = roleManager;
      _userRepo = userRepo;
      _vendorRepo = vendorRepo;
      _programRepo = programRepo;
      _deptRepo = deptRepo;
    }

    #endregion

    #region Method

    #region Public

    [HttpGet]
    [Route("{id}/Password")]
    public async Task<IActionResult> Password(int id) {
      var entity = await _userRepo.FindByIdAsync(id);
      if (entity == null) {
        _ThrowNotFoundError(id);
      }
      ViewBag.UserName = entity.UserName;
      return View(new Credential { Id = id });
    }

    [HttpPost]
    [Route("{id}/Password")]
    public async Task<IActionResult> Password(int id, Credential model) {
      if (!ModelState.IsValid) {
        return View(model);
      }
      var entity = await _userManager.FindByIdAsync(id.ToString());
      if (entity == null) {
        _ThrowNotFoundError(id);
      }
      entity.UpdatedAt = DateTime.UtcNow.ToAppDateTime();
      await _userManager.RemovePasswordAsync(entity);
      await _userManager.AddPasswordAsync(entity, model.Password);
      return RedirectToAction(nameof(Get))
        .WithSuccess("Success", "Password have been successfully changed");
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Get(KeywordParam paramModel) {
      var param = _mapper.Map<KeywordParam, Keyword>(paramModel);
      var entities = await _userRepo.FindListAsync(param);
      var model = _mapper.Map<QueryResult<User>, QueryResultModel<UserList>>(entities);
      var url = Url.Action(nameof(Get));
      model.Url = $"{url}?search={paramModel.Search}&page=:";
      ViewBag.Keyword = paramModel.Search;
      return View(model);
    }

    [HttpGet]
    [Route("Add")]
    public async Task<IActionResult> Add() {
      var model = new UserAdd();
      var entity = new User();
      _mapper.Map<User, UserModel>(entity, model);
      model.Departments = _mapper.Map<IList<Department>, IList<SelectListItem>>(await _deptRepo.FindListAsync());
      model.Vendors = _mapper.Map<IList<Vendor>, IList<UserSelection>>(await _vendorRepo.FindListAsync());
      model.Programs = _mapper.Map<IList<Entities.Program>, IList<UserSelection>>(await _programRepo.FindListAsync());
      model.Roles = _mapper.Map<IList<Role>, IList<UserSelection>>(await _roleManager.Roles.ToListAsync());
      return View(model);
    }

    [HttpPost]
    [Route("Add")]
    public async Task<IActionResult> Add(UserAdd model) {
      if (!ModelState.IsValid) {
        model.Departments = _mapper.Map<IList<Department>, IList<SelectListItem>>(await _deptRepo.FindListAsync());
        return View(model);
      }
      var entity = _mapper.Map<UserModel, User>(model);
      entity.CreatedAt = DateTime.UtcNow.ToAppDateTime();
      entity.UpdatedAt = DateTime.UtcNow.ToAppDateTime();
      var result = await _userManager.CreateAsync(entity, model.Password);
      if (!result.Succeeded) {
        foreach (var error in result.Errors) {
          ModelState.AddModelError(string.Empty, error.Description);
        }
        return View(model);
      }
      return RedirectToAction(nameof(Get))
        .WithSuccess("Success", "User have been successfully created");
    }

    [HttpPost]
    [Route("{id}/Delete")]
    public async Task<IActionResult> Delete(int id) {
      var entity = await _userManager.FindByIdAsync(id.ToString());
      if (entity == null) {
        _ThrowNotFoundError(id);
      }
      if (int.Parse(_userManager.GetUserId(User)) == id || await _userRepo.IsInUse(id)) {
        _ThrowIsInUseError(id);
      }
      await _userManager.DeleteAsync(entity);
      return RedirectToAction(nameof(Get))
        .WithSuccess("Success", "User have been successfully deleted");
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Edit(int id) {
      var entity = await _userRepo.FindByIdAsync(id);
      if (entity == null) {
        _ThrowNotFoundError(id);
      }
      var dbPrograms = await _programRepo.FindListAsync();
      var departments = _mapper.Map<IList<Department>, IList<SelectListItem>>(await _deptRepo.FindListAsync());
      var vendors = _mapper.Map<IList<Vendor>, IList<UserSelection>>(await _vendorRepo.FindListAsync());
      var programs = _mapper.Map<IList<Entities.Program>, IList<UserSelection>>(dbPrograms);
      var roles = _mapper.Map<IList<Role>, IList<UserSelection>>(await _roleManager.Roles.ToListAsync());
      foreach (var e in entity.UserVendors) {
        vendors.First(m => m.Id == e.VendorId).IsSelected = true;
      }
      foreach (var e in entity.UserPrograms) {
        programs.First(m => m.Id == e.ProgramId).IsSelected = true;
      }
      foreach (var e in entity.UserRoles) {
        roles.First(m => m.Id == e.RoleId).IsSelected = true;
      }
      var vendorRole = roles.First(v => v.Id == Roles.Vendor.Id);
      var adminRole = roles.First(v => v.Id == Roles.Admin.Id);
      var superAdminRole = roles.First(v => v.Id == Roles.SuperAdmin.Id);
      vendorRole.Enabled = !adminRole.IsSelected && !superAdminRole.IsSelected;
      adminRole.Enabled = !vendorRole.IsSelected;
      superAdminRole.Enabled = !vendorRole.IsSelected;
      foreach (var p in programs) {
        p.Enabled = dbPrograms.First(e => e.Id == p.Id).Enabled && (vendorRole.IsSelected || adminRole.IsSelected);
      }
      foreach (var v in vendors) {
        v.Enabled = vendorRole.IsSelected || adminRole.IsSelected;
      }
      var model = new UserModel();
      _mapper.Map<User, UserModel>(entity, model);
      model.Departments = departments;
      model.Vendors = vendors;
      model.Programs = programs;
      model.Roles = roles;
      return View(model);
    }

    [HttpPost]
    [Route("{id}")]
    public async Task<IActionResult> Edit(int id, UserModel model) {
      if (!ModelState.IsValid) {
        model.Departments = _mapper.Map<IList<Department>, IList<SelectListItem>>(await _deptRepo.FindListAsync());
        return View(model);
      }
      var entity = await _userRepo.FindByIdAsync(id);
      if (entity == null) {
        _ThrowNotFoundError(id);
      }
      entity.UpdatedAt = DateTime.UtcNow.ToAppDateTime();
      _mapper.Map<UserModel, User>(model, entity);
      await _userManager.UpdateAsync(entity);
      var message = int.Parse(_userManager.GetUserId(User)) == id ?
        "You must re-login for the change to take effect" :
        "User have been successfully updated";
      return RedirectToAction(nameof(Get)).WithSuccess("Success", message);
    }

    #endregion

    #region Private

    private void _ThrowNotFoundError(int id) {
      throw new AppException($"User with Id = {id} could not be found", StatusCodes.Status404NotFound);
    }

    private void _ThrowIsInUseError(int id) {
      throw new AppException($"User with Id = {id} is in use", StatusCodes.Status403Forbidden);
    }

    #endregion

    #endregion

  }
}