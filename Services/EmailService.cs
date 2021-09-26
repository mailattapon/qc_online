using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using QcSupplier.Constants;
using QcSupplier.Entities;
using QcSupplier.Infrastructures;
using QcSupplier.Models;
using QcSupplier.Repositories;
using QcSupplier.Settings;
using QcSupplier.ViewModels;

namespace QcSupplier.Services {
  public class EmailService {
    #region Field

    private readonly EmailQueue _queue;
    private readonly RazorViewToStringRenderer _renderer;
    private readonly VendorRepository _vendorRepo;
    private readonly UserRepository _userRepo;
    private readonly EmailSetting _emailSetting;
    private readonly SupplierEvaluationRepository _supplierEvaluationRepo;
    private readonly ILogger<EmailService> _logger;
    private readonly OutGoingDataRepository _outgoingDataRepo;

    private readonly IMapper _mapper;

    #endregion

    #region Constructor

    public EmailService(
      EmailQueue queue,
      RazorViewToStringRenderer renderer,
      VendorRepository vendorRepo,
      UserRepository userRepo,
      EmailSetting emailSetting,
      SupplierEvaluationRepository supplierEvaluationRepo,
      ILogger<EmailService> logger,
      OutGoingDataRepository outgoingDataRepo,
      IMapper mapper
    ) {
      _queue = queue;
      _renderer = renderer;
      _vendorRepo = vendorRepo;
      _userRepo = userRepo;
      _emailSetting = emailSetting;
      _supplierEvaluationRepo = supplierEvaluationRepo;
      _logger = logger;
      _outgoingDataRepo = outgoingDataRepo;
      _mapper = mapper;
    }

    #endregion

    #region Method

    #region Public

    public async Task SentOutGoingDataVendorMailAsync(int id) {
            if (!_emailSetting.Enabled)
            {
                return;
            }
            var entity = await _outgoingDataRepo.FindByIdAsync(id);
      var users = await _userRepo.FindVendorListByVendorAndProgramAsync(entity.VendorId, Programs.OutGoingData.Id);
      if (users.Count == 0) {
        return;
      }
      var path = "/Views/Emails/Mail8.cshtml";
      var body = await _renderer.RenderViewToStringAsync(path, entity);
      var subject = "TNS Inspection your data not PASS";
      var recipients = _GetMailList(users);
      _logger.LogInformation("Sending Mail 8 into queue...");
      _SendToQueue(subject, recipients, body);
    }

    public async Task SendOutGoingDataTnsMailAsync(OutGoingData entity) {
      if (!_emailSetting.Enabled) {
        return;
      }
      var users = await _userRepo.FindTnsListByVendorAndProgramAsync(entity.VendorId, Programs.OutGoingData.Id);
      if (users.Count == 0) {
        return;
      }
      entity.Vendor = await _vendorRepo.FindByIdAsync(entity.VendorId);
      var path = "/Views/Emails/Mail7.cshtml";
      var body = await _renderer.RenderViewToStringAsync(path, entity);
      var subject = $"{entity.Vendor.Name} : Inspection data is updated";
      var recipients = _GetMailList(users);
      _logger.LogInformation("Sending Mail 7 into queue...");
      _SendToQueue(subject, recipients, body);
    }

    public async Task SendSupplierInformationTnsMailAsync(SupplierInformation entity) {
      if (!_emailSetting.Enabled) {
        return;
      }
      var users = await _userRepo.FindTnsListByVendorAndProgramAsync(entity.VendorId, Programs.SupplierInformation.Id);
      if (users.Count == 0) {
        return;
      }
      entity.Vendor = await _vendorRepo.FindByIdAsync(entity.VendorId);
      var path = "/Views/Emails/Mail6.cshtml";
      var body = await _renderer.RenderViewToStringAsync(path, entity);
      var subject = $"{entity.Vendor.Name} : Supplier information is updated";
      var recipients = _GetMailList(users);
      _logger.LogInformation("Sending Mail 6 into queue...");
      _SendToQueue(subject, recipients, body);
    }

    public async Task SendSupplierEvaluationVendorMailAsync(SupplierEvaluation entity) {
      if (!_emailSetting.Enabled) {
        return;
      }
      var users = await _userRepo.FindVendorListByVendorAndProgramAsync(entity.VendorId, Programs.SupplierEvaluation.Id);
      if (users.Count == 0) {
        return;
      }
      var path = "/Views/Emails/Mail1.cshtml";
      var body = await _renderer.RenderViewToStringAsync(path, entity);
      var subject = "TNS Supplier Evaluation Informed";
      var recipients = _GetMailList(users);
      _logger.LogInformation("Sending Mail 1 into queue...");
      _SendToQueue(subject, recipients, body);
    }

    public async Task SendSupplierEvaluationTnsMailAsync(SupplierEvaluation entity) {
      if (!_emailSetting.Enabled) {
        return;
      }
      var users = await _userRepo.FindTnsListByVendorAndProgramAsync(entity.VendorId, Programs.SupplierEvaluation.Id);
      if (users.Count == 0) {
        return;
      }
      entity.Vendor = await _vendorRepo.FindByIdAsync(entity.VendorId);
      var path = "/Views/Emails/Mail2.cshtml";
      var body = await _renderer.RenderViewToStringAsync(path, entity);
      var subject = $"{entity.Vendor.Name} : Supplier Update Data Informed ";
      var recipients = _GetMailList(users);
      _logger.LogInformation("Sending Mail 2 into queue...");
      _SendToQueue(subject, recipients, body);
    }

    public async Task SendDownloadMailAsync(Download entity) {
      if (!_emailSetting.Enabled) {
        return;
      }
      var users = await _userRepo.FindVendorListByProgramAsync(Programs.Download.Id);
      if (users.Count == 0) {
        return;
      }
      var path = "/Views/Emails/Mail4.cshtml";
      var body = await _renderer.RenderViewToStringAsync(path, entity);
      var subject = "TNS Data download is updated";
      var recipients = _GetMailList(users);
      _logger.LogInformation("Sending Mail 4 into queue...");
      _SendToQueue(subject, recipients, body);
    }

    public async Task SendTnsFormMailAsync(TnsForm entity) {
      if (!_emailSetting.Enabled) {
        return;
      }
      var users = await _userRepo.FindVendorListByProgramAsync(Programs.TnsForm.Id);
      if (users.Count == 0) {
        return;
      }
      var path = "/Views/Emails/Mail5.cshtml";
      var body = await _renderer.RenderViewToStringAsync(path, entity);
      var subject = "TNS New Form is updated";
      var recipients = _GetMailList(users);
      _logger.LogInformation("Sending Mail 5 into queue...");
      _SendToQueue(subject, recipients, body);
    }

    public async Task SendSelfControlledIPPMailAsync(SelfControlledIPP entity) {
      if (!_emailSetting.Enabled) {
        return;
      }
      var users = await _userRepo.FindTnsListByVendorAndProgramAsync(entity.VendorId, Programs.SelfControlledIpp.Id);
      if (users.Count == 0) {
        return;
      }
      entity.Vendor = await _vendorRepo.FindByIdAsync(entity.VendorId);
      var path = "/Views/Emails/Mail9.cshtml";
      var body = await _renderer.RenderViewToStringAsync(path, entity);
      var subject = "TNS Self-Controlled update information";
      var recipients = _GetMailList(users);
      _logger.LogInformation("Sending Mail 9 into queue...");
      _SendToQueue(subject, recipients, body);
    }

    public async Task SendSupplierEvaluationReminderMailAsync() {
      if (!_emailSetting.Enabled) {
        return;
      }
      var vendorIds = await _supplierEvaluationRepo.GetVendorsForSupplierEvaluationReminderAsync();
      if (vendorIds.Count == 0) {
        return;
      }
      var users = await _userRepo.FindVendorListByVendorsAndProgramAsync(vendorIds, Programs.SupplierEvaluation.Id);
      var path = "/Views/Emails/Mail3.cshtml";
      var body = await _renderer.RenderViewToStringAsync(path, false);
      var subject = "TNS Supplier Evaluate Feedback Alert";
      var recipients = _GetMailList(users);
      _logger.LogInformation("Sending Mail 3 into queue...");
      _SendToQueue(subject, recipients, body);
    }

    public async Task SendSelfControlledIPPReminderMailAsync() {
      if (!_emailSetting.Enabled) {
        return;
      }
      var vendorIds = await _vendorRepo.GetListForSelfControlledIPPReminderAsync();
      if (vendorIds.Count == 0) {
        return;
      }
      var users = await _userRepo.FindVendorListByVendorsAndProgramAsync(vendorIds, Programs.SelfControlledIpp.Id);
      var path = "/Views/Emails/Mail10.cshtml";
      var body = await _renderer.RenderViewToStringAsync(path, false);
      var subject = "TNS Self-Controlled IPP update Alert";
      var recipients = _GetMailList(users);
      _logger.LogInformation("Sending Mail 10 into queue...");
      _SendToQueue(subject, recipients, body);
    }

    //2021/09/15 lattapon
    public async Task SendOutGoingDataTnsMailAsync_new(OutGoingDatas_NewList entity, string VendorName, string VendorEmail)
    {
        if (!_emailSetting.Enabled)
        {
            return;
        }
        var users = await _userRepo.FindTnsListByVendorAndProgramAsync(entity.Id, Programs.OutGoingData.Id);
        if (users.Count == 0)
        {
            return;
        }
        var path = "/Views/Emails/Mail11.cshtml";
        //var model = _mapper.Map<QueryResult<OutGoingDatas_New>, QueryResultModel<OutGoingDatas_NewList>>(entity);
        var body = await _renderer.RenderViewToStringAsync(path, entity);

        IList<string> ilist = new List<string>();
        ilist.Add(VendorEmail);
        var subject = $"{VendorName} : Inspection data is updated";
        _logger.LogInformation("Sending Mail 11 into queue...");
        _SendToQueue(subject, ilist, body);
    }

        #endregion

    #region Private

    private IList<string> _GetMailList(IList<User> users) {
      return users.Select(u => u.Email).Where(e => !string.IsNullOrEmpty(e)).Distinct().ToList();
    }

    private void _SendToQueue(string subject, IList<string> recipients, string body) {
      var task = new EmailTask {
        Subject = subject,
        Recipients = recipients,
        Body = body
      };
      _queue.Enqueue(task);
    }


        #endregion

        #endregion
    }
}