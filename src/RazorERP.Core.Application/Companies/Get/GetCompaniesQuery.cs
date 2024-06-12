using MediatR;

namespace RazorERP.Core.Application.Companies.Get
{
    public record GetCompaniesQuery() : IRequest<List<CompanyResponse>>;
}
