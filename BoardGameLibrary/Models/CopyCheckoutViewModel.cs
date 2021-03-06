﻿using BoardGameLibrary.Data.Models;
using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BoardGameLibrary.Models
{
    [Validator(typeof(CopyCheckoutValidator))]
    public class CopyCheckOutViewModel
    {
        [Display(Name = "Library ID #")]
        public string CopyLibraryID { get; set; }
        [Display(Name = "Attendee Badge #")]
        public string AttendeeBadgeID { get; set; }
        [Display(Name = "Override Limit")]
        public bool OverrideLimit { get; set; }
        public IList<string> Messages { get; set; }

        public CopyCheckOutViewModel()
        {
            OverrideLimit = false;
            Messages = new List<string>();
        }
    }

    public class CopyCheckoutValidator : AbstractValidator<CopyCheckOutViewModel>
    {
        ApplicationDbContext _db;
        public CopyCheckoutValidator()
        {
            _db = new ApplicationDbContext();
            var gameAlreadyCheckedOut = "";
            RuleFor(x => x.AttendeeBadgeID).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Badge ID required.")
                .Must(BeAnExistingAttendee).WithMessage("Attendee not found.");
                
            Unless(a => a.OverrideLimit, () => {
                RuleFor(x => x.AttendeeBadgeID).Cascade(CascadeMode.StopOnFirstFailure)
                    .Must(badgeId => NotAlreadyHaveACopyCheckedOut(badgeId, out gameAlreadyCheckedOut))
                                               .WithMessage("Attendee has {0} checked out.", x => gameAlreadyCheckedOut);
            });

            RuleFor(x => x.CopyLibraryID).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("You must provide a library ID.")
                .Must(BeANumber).WithMessage("Library IDs must be a number")
                .Must(BeAnExistingGameCopy).WithMessage("Copy not found.")
                .Must(NotBeCheckedOut).WithMessage("That copy is checked out already.  Check it in first.");
        }

        private bool BeANumber(string attendeeBadgeID)
        {
            int parsedInt = 0;
            var parsedSuccessfully = Int32.TryParse(attendeeBadgeID, out parsedInt);

            return parsedSuccessfully;
        }

        private bool BeAnExistingAttendee(string attendeeBadgeID)
        {
            if (_db.Attendees.AsNoTracking().FirstOrDefault(a => a.BadgeID == attendeeBadgeID) == null)
                return false;

            return true;
        }

        private bool NotAlreadyHaveACopyCheckedOut(string attendeeBadgeID, out string gameCheckedOutAlready)
        {
            var currentlyCheckedOutCopy = _db.Copies.AsNoTracking().FirstOrDefault(c => c.CurrentCheckout.Attendee.BadgeID == attendeeBadgeID);
            if (currentlyCheckedOutCopy != null)
            {
                gameCheckedOutAlready = currentlyCheckedOutCopy.Game.Title + "(#" + currentlyCheckedOutCopy.LibraryID + ")";
                return false;
            }

            gameCheckedOutAlready = "";
            return true;
        }

        private bool BeAnExistingGameCopy(string copyLibraryID)
        {
            var copyLibraryIDInt = Convert.ToInt32(copyLibraryID.Replace("*", ""));
            if (_db.Copies.AsNoTracking().FirstOrDefault(c => c.LibraryID == copyLibraryIDInt) == null)
                return false;

            return true;
        }

        private bool NotBeCheckedOut(string copyLibraryID)
        {
            var copyLibraryIDInt = Convert.ToInt32(copyLibraryID.Replace("*", ""));
            var copy = _db.Copies.AsNoTracking().FirstOrDefault(c => c.LibraryID == copyLibraryIDInt);
            if (copy.CurrentCheckout != null)
                return false;

            return true;
        }
    }
}