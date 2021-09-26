using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QcSupplier.Constants;
using QcSupplier.Entities;
using QcSupplier.Extensions;
using QcSupplier.Models;
using QcSupplier.Persistence;
using QcSupplier.Repositories;
using QcSupplier.ViewModels;

namespace QcSupplier.Controllers {

  [Route("[Controller]")]
  [Authorize(Roles = Roles.SUPER_ADMIN)]
  public class VendorController : Controller {
    #region Field

    private readonly IMapper _mapper;
    private readonly UnitOfWork _unitOfWork;
    private readonly VendorRepository _vendorRepo;

    #endregion

    #region Constructor

    public VendorController(IMapper mapper, UnitOfWork unitOfWork, VendorRepository vendorRepo) {
      _mapper = mapper;
      _unitOfWork = unitOfWork;
      _vendorRepo = vendorRepo;
    }

    #endregion

    #region Method

    #region Public

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Get(KeywordParam paramModel) {
      var param = _mapper.Map<KeywordParam, Keyword>(paramModel);
      var entities = await _vendorRepo.FindListAsync(param);
      var model = _mapper.Map<QueryResult<Vendor>, QueryResultModel<VendorModel>>(entities);
      var url = Url.Action(nameof(Get));
      model.Url = $"{url}?search={paramModel.Search}&page=:";
      ViewBag.Keyword = paramModel.Search;
      return View(model);
    }

    [HttpGet]
    [Route("Add")]
    public IActionResult Add() {
      var entity = new Vendor();
      return View(_mapper.Map<Vendor, VendorModel>(entity));
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Edit(int id) {
      var entity = await _vendorRepo.FindByIdAsync(id);
      if (entity == null) {
        _ThrowNotFoundError(id);
      }
      var model = _mapper.Map<Vendor, VendorModel>(entity);
      return View(model);
    }

    [HttpPost]
    [Route("{id}")]
    public async Task<IActionResult> Edit(int id, VendorModel model) {
      if (!ModelState.IsValid) {
        return View(model);
      }
      var entity = await _vendorRepo.FindByIdAsync(id);
      _mapper.Map<VendorModel, Vendor>(model, entity);
      entity.UpdatedAt = DateTime.UtcNow.ToAppDateTime();
      await _unitOfWork.CompleteAsync();
      return RedirectToAction(nameof(Get))
        .WithSuccess("Success", "Vendor have been successfully updated");
    }

    [HttpPost]
    [Route("Add")]
    public async Task<IActionResult> Add(VendorModel model) {
      if (!ModelState.IsValid) {
        return View(model);
      }
      var entity = _mapper.Map<VendorModel, Vendor>(model);
      entity.UpdatedAt = DateTime.UtcNow.ToAppDateTime();
      entity.CreatedAt = DateTime.UtcNow.ToAppDateTime();
      _vendorRepo.Add(entity);
      await _unitOfWork.CompleteAsync();
      return RedirectToAction(nameof(Get))
        .WithSuccess("Success", "Vendor have been successfully created");
    }

    [HttpPost]
    [Route("{id}/Delete")]
    public async Task<IActionResult> Delete(int id) {
      var entity = await _vendorRepo.FindByIdAsync(id);
      if (entity == null) {
        _ThrowNotFoundError(id);
      }
      if (await _vendorRepo.IsInUse(id)) {
        _ThrowIsInUseError(id);
      }
      _vendorRepo.Remove(entity);
      await _unitOfWork.CompleteAsync();
      return RedirectToAction(nameof(Get))
        .WithSuccess("Success", "Vendor have been successfully deleted");
    }

    #endregion

    #region Private

    #endregion

    private void _ThrowNotFoundError(int id) {
      throw new AppException($"Vendor with Id = {id} could not be found", StatusCodes.Status404NotFound);
    }

    private void _ThrowIsInUseError(int id) {
      throw new AppException($"Vendor with Id = {id} is in use", StatusCodes.Status403Forbidden);
    }

    #endregion

  }
}