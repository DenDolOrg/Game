namespace MyGame.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Player
    {  
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public virtual Login Login { get; set; }
    }
}
