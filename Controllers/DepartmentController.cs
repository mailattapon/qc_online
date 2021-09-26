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
  public class DepartmentController : Controller {
    #region Field

    private readonly IMapper _mapper;
    private readonly UnitOfWork _unitOfWork;
    private readonly DepartmentRepository _deptRepo;

    #endregion

    #region Constructor

    public DepartmentController(IMapper mapper, UnitOfWork unitOfWork, DepartmentRepository deptRepo) {
      _unitOfWork = unitOfWork;
      _deptRepo = deptRepo;
      _mapper = mapper;
    }

    #endregion

    #region Method

    #region Public

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Get(KeywordParam paramModel) {
      var param = _mapper.Map<KeywordParam, Keyword>(paramModel);
      var entities = await _deptRepo.FindListAsync(param);
      var model = _mapper.Map<QueryResult<Department>, QueryResultModel<DepartmentModel>>(entities);
      var url = Url.Action(nameof(Get));
      model.Url = $"{url}?search={paramModel.Search}&page=:";
      ViewBag.Keyword = paramModel.Search;
      return View(model);
    }

    [HttpGet]
    [Route("Add")]
    public IActionResult Add() {
      var entity = new Department();
      return View(_mapper.Map<Department, DepartmentModel>(entity));
    }

    [HttpPost]
    [Route("Add")]
    public async Task<IActionResult> Add(DepartmentModel model) {
      if (!ModelState.IsValid) {
        return View(model);
      }
      var entity = _mapper.Map<DepartmentModel, Department>(model);
      entity.CreatedAt = DateTime.UtcNow.ToAppDateTime();
      entity.UpdatedAt = DateTime.UtcNow.ToAppDateTime();
      _deptRepo.Add(entity);
      await _unitOfWork.CompleteAsync();
      return RedirectToAction(nameof(Get))
        .WithSuccess("Success", "Department have been successfully created");
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Edit(int id) {
      var entity = await _deptRepo.FindByIdAsync(id);
      if (entity == null) {
        _ThrowNotFoundError(id);
      }
      var model = _mapper.Map<Department, DepartmentModel>(entity);
      return View(model);
    }

    [HttpPost]
    [Route("{id}")]
    public async Task<IActionResult> Edit(int id, DepartmentModel model) {
      if (!ModelState.IsValid) {
        return View(model);
      }
      var entity = await _deptRepo.FindByIdAsync(id);
      _mapper.Map<DepartmentModel, Department>(model, entity);
      entity.UpdatedAt = DateTime.UtcNow.ToAppDateTime();
      await _unitOfWork.CompleteAsync();
      return RedirectToAction(nameof(Get))
        .WithSuccess("Success", "Department have been successfully updated");
    }

    [HttpPost]
    [Route("{id}/Delete")]
    public async Task<IActionResult> Delete(int id) {
      var entity = await _deptRepo.FindByIdAsync(id);
      if (entity == null) {
        _ThrowNotFoundError(id);
      }
      if (await _deptRepo.IsInUse(id)) {
        _ThrowIsInUseError(id);
      }
      _deptRepo.Remove(entity);
      await _unitOfWork.CompleteAsync();
      return RedirectToAction(nameof(Get))
        .WithSuccess("Success", "Department have been successfully deleted");
    }

    #endregion

    #region Private

    private void _ThrowNotFoundError(int id) {
      throw new AppException($"Department with Id = {id} could not be found", StatusCodes.Status404NotFound);
    }

    private void _ThrowIsInUseError(int id) {
      throw new AppException($"Department with Id = {id} is in use", StatusCodes.Status403Forbidden);
    }

    #endregion

    #endregion

  }
}