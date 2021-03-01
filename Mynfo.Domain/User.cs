﻿namespace Mynfo.Domain
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "The field {0} is required.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contains a maxium of {1} characters lenght.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contains a maxium of {1} characters lenght.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [MaxLength(100, ErrorMessage = "The field {0} only can contains a maxium of {1} characters lenght.")]
        [Index("User_Email_Index", IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        public int UserTypeId { get; set; }

        public bool Share { get; set; }

        [Display(Name = "Edad")]
        public int Edad { get; set; }

        [Display(Name = "Ubicacion")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contains a maxium of {1} characters lenght.")]
        public string Ubicacion { get; set; }

        [Display(Name = "Ocupacion")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contains a maxium of {1} characters lenght.")]
        public string Ocupacion { get; set; }

        [Display(Name = "Conexiones")]
        public int Conexiones { get; set; }

        [JsonIgnore]
        public virtual UserType UserType { get; set; }

        [NotMapped]
        public byte[] ImageArray { get; set; }

        [NotMapped]
        public string Password { get; set; }

        [Display(Name = "Image")]
        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImagePath))
                {
                    return "noimage";
                }
                return string.Format(
                    "https://mynfoapi.azurewebsites.net/{0}",
                    ImagePath.Substring(1));
            }
        }

        [Display(Name = "User")]
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }
        [JsonIgnore]
        public virtual ICollection<ProfileEmail> ProfilePhone { get; set; }

        [JsonIgnore]
        public virtual ICollection<ProfilePhone> ProfileEmail { get; set; }

        [JsonIgnore]
        public virtual ICollection<Box> Box { get; set; }

        [JsonIgnore]
        public virtual ICollection<ProfileSM> ProfileSM { get; set; }
    }
}
