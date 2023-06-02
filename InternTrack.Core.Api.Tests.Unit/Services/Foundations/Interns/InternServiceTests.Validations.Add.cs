﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternTrack.Core.Api.Models.Interns;
using InternTrack.Core.Api.Models.Interns.Exceptions;
using Moq;
using Xunit;

namespace InternTrack.Core.Api.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfInternIsNullAndLogItAsync()
        {
            // given
            Models.Interns.Intern nullIntern = null;

            var nullInternException =
                new NullInternException();

            var expectedInternValidationException =
                new InternValidationException(nullInternException);

            // when
            ValueTask<Models.Interns.Intern> createInternTask =
                this.internService.CreateInternAsync(nullIntern);

            // then
            await Assert.ThrowsAsync<InternValidationException>(() =>
                createInternTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedInternValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfInternIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidIntern = new Models.Interns.Intern
            {
                FirstName = invalidText,
                MiddleName = invalidText,
                LastName = invalidText,
                Email = invalidText,
                PhoneNumber = invalidText,
                Status = invalidText
            };

            var invalidInternException = new InvalidInternException();

            invalidInternException.AddData(
                key: nameof(Models.Interns.Intern.Id),
                values: "Id is required");

            invalidInternException.AddData(
                key: nameof(Models.Interns.Intern.FirstName),
                values: "Text is required");

            invalidInternException.AddData(
                key: nameof(Models.Interns.Intern.MiddleName),
                values: "Text is required");

            invalidInternException.AddData(
                key: nameof(Models.Interns.Intern.LastName),
                values: "Text is required");

            invalidInternException.AddData(
                key: nameof(Models.Interns.Intern.Email),
                values: "Text is required");

            invalidInternException.AddData(
                key: nameof(Models.Interns.Intern.PhoneNumber),
                values: "Text is required");

            invalidInternException.AddData(
                key: nameof(Models.Interns.Intern.Status),
                values: "Text is required");            

            invalidInternException.AddData(
                key: nameof(Models.Interns.Intern.CreatedDate),
                values: "Id is required");

            invalidInternException.AddData(
                key: nameof(Models.Interns.Intern.CreatedBy),
                values: "Id is required");

            invalidInternException.AddData(
                key: nameof(Models.Interns.Intern.UpdatedDate),
                values: "Id is required");

            invalidInternException.AddData(
                key: nameof(Models.Interns.Intern.UpdatedBy),
                values: "Id is required");

            var expectedInternValidationException =
                new InternValidationException(invalidInternException);

            // when
            ValueTask<Models.Interns.Intern> createInternTask =
                this.internService.CreateInternAsync(invalidIntern);

            // then
            await Assert.ThrowsAsync<InternValidationException>(() =>
                createInternTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedInternValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertInternAsync(It.IsAny<Models.Interns.Intern>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
