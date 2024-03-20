global using Adecco.API.Extensions;
global using Adecco.API.WelcomePage;
global using Adecco.Application.Dtos.Cliente;
global using Adecco.Application.Dtos.Contato;
global using Adecco.Application.Dtos.Endereco;
global using Adecco.Application.Services;
global using Adecco.Application.Validation;
global using Adecco.Core.Entities;
global using Adecco.Core.Interfaces.Repositories;
global using Adecco.Core.Interfaces.Services;
global using Adecco.Persistence.Contexts;
global using Adecco.Persistence.Repositories;

global using Asp.Versioning;
global using Asp.Versioning.ApiExplorer;

global using AutoMapper;

global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.ApiExplorer;
global using Microsoft.AspNetCore.Mvc.ModelBinding;
global using Microsoft.AspNetCore.Routing;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.OpenApi.Models;

global using Swashbuckle.AspNetCore.SwaggerGen;
global using Swashbuckle.AspNetCore.SwaggerUI;

global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text.Json;
global using System.Threading.Tasks;

global using Adecco.Core.Exceptions;
global using Adecco.Persistence.Extensions;