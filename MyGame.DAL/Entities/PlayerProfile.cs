using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyGame.DAL.Entities
{
    /// <summary>
    /// Model of Players profile. Table Profile will be created in DB.
    /// </summary>
    public class PlayerProfile
    {
        /// <summary>
        /// Profile id.
        /// </summary>
        [Key]
        [ForeignKey("ApplicationUser")]
        public virtual int Id { get; set; }

        /// <summary>
        /// Players name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Players surname.
        /// </summary>
        public virtual string Surname { get; set; }

        /// <summary>
        /// Foreign key. Contains link on appropriate user.
        /// </summary>
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
