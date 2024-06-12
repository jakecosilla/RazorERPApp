using RazorERP.Core.Domain.Companies;
using MediatR;
using System.Linq;

namespace RazorERP.Core.Application.Companies.Get
{
    internal sealed class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, List<CompanyResponse>>
    {
        private readonly ICompanyRepository _CompanyRepository;

        public GetCompaniesQueryHandler(ICompanyRepository CompanyRepository)
        {
            _CompanyRepository = CompanyRepository;
        }

        public async Task<List<CompanyResponse>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
        {
            var companies = await _CompanyRepository.Get();
           
            var result = companies.Select(Company => new CompanyResponse(
                Company.Id,
                Company.Name)).ToList();

            return result;
        }
    }
}
