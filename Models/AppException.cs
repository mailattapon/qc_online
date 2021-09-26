using System;
using Microsoft.AspNetCore.Http;

namespace QcSupplier.Models {
  public class AppException : Exception {
    #region Property

    public int StatusCode { get; }

    #endregion

    #region Constructor

    public AppException(string message, int statusCode = StatusCodes.Status500InternalServerError) : base(message) {
      StatusCode = statusCode;
    }

    #endregion
  }
}