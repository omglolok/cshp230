using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BirthdayCard.Models
{
    public class Message
    {
        [Required(ErrorMessage="Please enter From")]
        public string From { get; set; }
        [Required(ErrorMessage="Please enter To")]
        public string To { get; set; }
        [Required(ErrorMessage="Please enter a birthday message")]
        public string BirthdayMessage { get; set; }
    }
}