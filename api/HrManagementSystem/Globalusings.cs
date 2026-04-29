global using System.ComponentModel.DataAnnotations;
global using System.Globalization;
global using System.IdentityModel.Tokens.Jwt;
global using System.Linq.Dynamic.Core;
global using System.Reflection;
global using System.Security.Claims;
global using System.Security.Cryptography;
global using System.Text;
global using System.Threading.RateLimiting;
global using Asp.Versioning;
global using Asp.Versioning.ApiExplorer;
global using FluentValidation;
global using Hangfire;
global using HealthChecks.UI.Client;

global using Microsoft.EntityFrameworkCore.ChangeTracking;
global using Microsoft.EntityFrameworkCore.Diagnostics;
global using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

global using HrManagementSystem.Contracts.BasicContracts.Authorization.Roles;
global using HrManagementSystem.Services.BasicServices.RoleService;
global using HrManagementSystem.Services.StateService;

global using HrManagementSystem.Contracts.BasicContracts.ReportCategory;
global using HrManagementSystem.Services.BasicServices.ReportCategoryService;

global using HrManagementSystem.Contracts.BasicContracts.Users;
global using HrManagementSystem.Contracts.BasicContracts.UploadFiles;
global using HrManagementSystem.Contracts.BasicContracts.Localizations;
global using HrManagementSystem.Contracts.BasicContracts.SubCategories;
global using HrManagementSystem.Contracts.BasicContracts.Users.CreateUser;
global using HrManagementSystem.Contracts.BasicContracts.Users.UpdateUser;
global using HrManagementSystem.Contracts.Addresses;
global using HrManagementSystem.Contracts.States;

global using HrManagementSystem.Services.BasicServices.FileService;
global using HrManagementSystem.Services.BasicServices.LocalizationService;

global using HrManagementSystem.Services.BasicServices.SubCategoriesService;
global using HrManagementSystem.Services.BasicServices.Users;

global using HrManagementSystem.Contracts.BasicContracts.Views;
global using HrManagementSystem.Services.BasicServices.Views;

global using HrManagementSystem.Errors.EntitiesErrors;
global using HrManagementSystem.Authorization.Filters;
global using HrManagementSystem.Errors.ResultPattern;
global using HrManagementSystem.Persistance.Seeds;
global using HrManagementSystem.Authentication;
global using HrManagementSystem.Localization;
global using HrManagementSystem.Persistance;
global using HrManagementSystem.Extensions;
global using HrManagementSystem.Settings;
global using HrManagementSystem.Versions;
global using HrManagementSystem.Helpers;
global using HrManagementSystem.Consts;

global using HrManagementSystem.Dependencies;
global using HrManagementSystem.Hangfire.Filters;
global using HrManagementSystem.Hubs.GeneralHub;

global using HrManagementSystem.Entities.BasicEntities;
global using HrManagementSystem.Entities.ApplicationEntities.GeographicDetails;
global using HrManagementSystem.Entities.ApplicationEntities.EmployeeDetails;

global using HrManagementSystem.Services.BasicServices.EntityChangeLogService;
global using HrManagementSystem.Services.BasicServices.ExportExcelService;
global using HrManagementSystem.Services.BasicServices.ArchiveDataService;
global using HrManagementSystem.Services.BasicServices.CategoriesService;
global using HrManagementSystem.Services.BasicServices.DashboardService;
global using HrManagementSystem.Services.BasicServices.AuthService;
global using HrManagementSystem.Services.AddressTypesService;
global using HrManagementSystem.Services.DistrictsService;
global using HrManagementSystem.Services.CountriesService;

global using HrManagementSystem.Contracts.BasicContracts.Authentication.ResendConfirmationEmail;
global using HrManagementSystem.Contracts.BasicContracts.Authentication.ForgotPassword;
global using HrManagementSystem.Contracts.BasicContracts.Authentication.ResetPassword;
global using HrManagementSystem.Contracts.BasicContracts.Authentication.RefreshToken;
global using HrManagementSystem.Contracts.BasicContracts.Authentication.ConfirmEmail;
global using HrManagementSystem.Contracts.BasicContracts.Users.UpdateUserProfile;
global using HrManagementSystem.Contracts.BasicContracts.Authentication.Register;
global using HrManagementSystem.Contracts.BasicContracts.Users.ChangePassword;
global using HrManagementSystem.Contracts.BasicContracts.Authentication.Login;
global using HrManagementSystem.Contracts.BasicContracts.Categories;
global using HrManagementSystem.Contracts.BasicContracts.Dashboard;
global using HrManagementSystem.Contracts.AddressTypes;
global using HrManagementSystem.Contracts.Countries;
global using HrManagementSystem.Contracts.Districts;
global using HrManagementSystem.Contracts.State;

global using HrManagementSystem.Services.BasicServices.ApiKeysService;
global using HrManagementSystem.Services.BasicServices.CacheService;
global using HrManagementSystem.Services.BasicServices.ExportPdfService;
global using HrManagementSystem.Services.BasicServices.NotificationService;

global using Mapster;
global using MapsterMapper;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Diagnostics.HealthChecks;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Identity.UI.Services;
global using Microsoft.AspNetCore.Localization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.SignalR;
global using Microsoft.AspNetCore.WebUtilities;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.Extensions.Caching.Distributed;
global using Microsoft.Extensions.Caching.Hybrid;
global using Microsoft.Extensions.Localization;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;
global using Serilog;
global using Serilog.Context;
global using Swashbuckle.AspNetCore.SwaggerGen;

global using HrManagementSystem.Contracts.BasicContracts.KanbanCardAssignees;
global using HrManagementSystem.Contracts.BasicContracts.KanbanCards;
global using HrManagementSystem.Contracts.BasicContracts.KanbanCardAttachments;
global using HrManagementSystem.Contracts.BasicContracts.KanbanCardComments;
global using HrManagementSystem.Contracts.BasicContracts.BoardTaskAttachments;


