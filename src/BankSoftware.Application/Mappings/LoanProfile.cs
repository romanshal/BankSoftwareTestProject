using AutoMapper;
using BankSoftware.Application.Features.Loans.Commands.Create;
using BankSoftware.Application.Models.Dtos;
using BankSoftware.Domain.Constants.Enums;
using BankSoftware.Domain.Entities;

namespace BankSoftware.Application.Mappings
{
    internal sealed class LoanProfile : Profile
    {
        public LoanProfile()
        {
            CreateMap<Loan, LoanDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status == LoanStatus.Published));

            CreateMap<CreateLoanCommand, Loan>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => LoanStatus.Published));
        }
    }
}
