//using FluentAssertions;
//using TeamChoice.WebApis.Contracts.DTOs;
//using TeamChoice.WebApis.Contracts.DTOs.Transactions;
//using TeamChoice.WebApis.Domain.Exceptions;
//using TeamChoice.WebApis.Domain.Services.Transactions;

//namespace TeamChoice.TestCases;

//public sealed class TransactionValidatorTests
//{
//    private readonly TransactionValidator _validator = new();

//    // ------------------------------------------------------------------
//    // NULL & CORE FIELD TESTS
//    // ------------------------------------------------------------------

//    [Fact]
//    public void Validate_ShouldThrow_WhenRequestIsNull()
//    {
//        Action act = () => _validator.Validate(null);

//        act.Should()
//           .Throw<TransactionValidationException>()
//           .WithMessage("*cannot be null*");
//    }

//    [Theory]
//    [InlineData(null)]
//    [InlineData("")]
//    [InlineData("   ")]
//    public void Validate_ShouldThrow_WhenPartnerReferenceIsInvalid(string partnerRef)
//    {
//        var dto = CreateValidRequest();
//        dto.PartnerReference = partnerRef;

//        Action act = () => _validator.Validate(dto);

//        act.Should().Throw<TransactionValidationException>();
//    }

//    // ------------------------------------------------------------------
//    // PAYMENT VALIDATION
//    // ------------------------------------------------------------------

//    [Fact]
//    public void Validate_ShouldThrow_WhenPaymentIsNull()
//    {
//        var dto = CreateValidRequest();
//        dto.Payment = null;

//        Action act = () => _validator.Validate(dto);

//        act.Should().Throw<TransactionValidationException>()
//           .WithMessage("*Payment*");
//    }

//    [Theory]
//    [InlineData(0)]
//    [InlineData(-1)]
//    public void Validate_ShouldThrow_WhenSenderAmountIsNotPositive(decimal amount)
//    {
//        var dto = CreateValidRequest();
//        dto.Payment.SenderAmount = amount;

//        Action act = () => _validator.Validate(dto);

//        act.Should().Throw<TransactionValidationException>()
//           .WithMessage("*amount*");
//    }

//    // ------------------------------------------------------------------
//    // SENDER VALIDATION
//    // -------------------------

//    // ------------------------------------------------------------------
//    // TEST DATA BUILDER
//    // ------------------------------------------------------------------

//    private static TransactionRequestDto CreateValidRequest()
//    {
//        return new TransactionRequestDto
//        {
//            PartnerReference = "REF123",
//            Purpose = "Salary",
//            Relationship = "Friend",

//            Payment = new PaymentDto
//            {
//                SenderAmount = 100,
//                PaymentMode = "CASH"
//            },

//            Sender = new PersonDto
//            {
//                FirstName = "John",
//                LastName = "Doe",
//                MobilePhone = "123456789"
//            },

//            Recipient = new WebApis.Contracts.DTOs.Transactions.RecipientDto
//            {
//                FirstName = "Jane",
//                LastName = "Doe",
//                MobilePhone = "987654321"
//            },

//            SendingLocation = new LocationDto
//            {
//                LocationId = "LOC01"
//            }
//        };
//    }
//}
